using Investment_Tracker.Data;
using Investment_Tracker.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Investment_Tracker.Services
{
    public class PortfolioService
    {
        private readonly AppDbContext _db;
        private readonly PriceRefreshService _prices;

        public PortfolioService(AppDbContext db, PriceRefreshService prices)
        {
            _db = db;
            _prices = prices;
        }

        // Lista investitiilor, fiecare cu P&L si datele pentru graficul din card.
        public async Task<List<InvestmentResponseDto>> GetInvestmentsWithPnlAsync(string userId)
        {
            var (rows, series, today) = await LoadPortfolioAsync(userId);
            return rows.Select(r => ToDto(r, series[r.AssetId], today)).ToList();
        }

        // O singura investitie (pagina de detalii). null daca nu exista sau nu e a utilizatorului.
        public async Task<InvestmentResponseDto?> GetInvestmentAsync(string userId, int investmentId)
        {
            var (rows, series, today) = await LoadPortfolioAsync(userId, investmentId);
            var row = rows.FirstOrDefault();
            return row is null ? null : ToDto(row, series[row.AssetId], today);
        }

        // Sumarul portofoliului + graficul valorii in timp.
        public async Task<PortfolioSummaryDto> GetSummaryAsync(string userId)
        {
            var (rows, series, today) = await LoadPortfolioAsync(userId);

            if (rows.Count == 0)
                return new PortfolioSummaryDto();   // toate valorile 0, liste goale

            var totalCostBasis = rows.Sum(r => r.Quantity * r.BuyPrice);
            var totalCurrentValue = rows.Sum(r => r.Quantity * (r.LastPrice ?? 0m));
            var totalPnlUsd = totalCurrentValue - totalCostBasis;
            var totalPnlPct = totalCostBasis == 0 ? 0 : (totalPnlUsd / totalCostBasis) * 100;

            var summary = new PortfolioSummaryDto
            {
                TotalCostBasis = totalCostBasis,
                TotalCurrentValue = totalCurrentValue,
                TotalPnlUsd = totalPnlUsd,
                TotalPnlPct = totalPnlPct
            };

            BuildPortfolioChart(rows, series, today, summary);
            return summary;
        }

        // ---------------------------------------------------------------
        // Incarcare date
        // ---------------------------------------------------------------

        private async Task<(List<InvestmentRow> Rows, Dictionary<int, PriceSeries> Series, DateOnly Today)>
            LoadPortfolioAsync(string userId, int? onlyInvestmentId = null)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var rows = await LoadInvestmentsAsync(userId, onlyInvestmentId);

            if (rows.Count == 0)
                return (rows, new Dictionary<int, PriceSeries>(), today);

            // Plasa de siguranta: daca un asset nu are inca pret salvat, il aducem o data.
            var withoutPrice = rows.Where(r => r.LastPrice is null).Select(r => r.AssetId).Distinct().ToList();
            if (withoutPrice.Count > 0)
            {
                await _prices.EnsureLatestPricesAsync(withoutPrice);
                rows = await LoadInvestmentsAsync(userId, onlyInvestmentId);
            }

            // Tot istoricul necesar, intr-un singur query.
            // Luam si cateva zile inainte de prima cumparare, ca sa avem un pret
            // "anterior" pentru weekend-uri / sarbatori (bursa inchisa).
            var assetIds = rows.Select(r => r.AssetId).Distinct().ToList();
            var from = rows.Min(r => r.BuyDate).AddDays(-7);

            var points = await _db.PriceHistory
                .Where(p => assetIds.Contains(p.assetId) && p.date >= from)
                .Select(p => new { p.assetId, p.date, p.close })
                .ToListAsync();

            var series = assetIds.ToDictionary(
                id => id,
                id => new PriceSeries(points.Where(p => p.assetId == id).Select(p => (p.date, p.close))));

            // Punctul de azi = ultimul pret cunoscut (acelasi folosit in P&L),
            // ca graficul sa se termine exact la valoarea afisata in card.
            foreach (var r in rows.Where(r => r.LastPrice is not null).GroupBy(r => r.AssetId).Select(g => g.First()))
                series[r.AssetId].Set(today, r.LastPrice!.Value);

            return (rows, series, today);
        }

        private async Task<List<InvestmentRow>> LoadInvestmentsAsync(string userId, int? onlyInvestmentId)
        {
            var query = _db.Investments.Where(i => i.userId == userId);
            if (onlyInvestmentId is not null)
                query = query.Where(i => i.id == onlyInvestmentId);

            return await query
                .OrderBy(i => i.id)
                .Select(i => new InvestmentRow
                {
                    Id = i.id,
                    AssetId = i.assetId,
                    AssetCode = i.asset.code,
                    AssetName = i.asset.name,
                    LastPrice = i.asset.lastPrice,
                    LastPriceAt = i.asset.lastPriceAt,
                    Quantity = i.quantity,
                    BuyPrice = i.buyPrice,
                    BuyDate = i.buyDate
                })
                .ToListAsync();
        }

        // ---------------------------------------------------------------
        // Calcule
        // ---------------------------------------------------------------

        private static InvestmentResponseDto ToDto(InvestmentRow r, PriceSeries series, DateOnly today)
        {
            // Daca tot nu avem pret, folosim 0 (la fel ca inainte)
            var currentPrice = r.LastPrice ?? 0m;

            var costBasis = r.Quantity * r.BuyPrice;
            var currentValue = r.Quantity * currentPrice;
            var pnlUsd = currentValue - costBasis;
            var pnlPct = costBasis == 0 ? 0 : (pnlUsd / costBasis) * 100;

            var dto = new InvestmentResponseDto
            {
                Id = r.Id,
                AssetCode = r.AssetCode,
                AssetName = r.AssetName,
                Quantity = r.Quantity,
                BuyPrice = r.BuyPrice,
                BuyDate = r.BuyDate,
                CurrentPrice = currentPrice,
                PriceUpdatedAt = r.LastPriceAt,
                CostBasis = costBasis,
                CurrentValue = currentValue,
                PnlUsd = pnlUsd,
                PnlPct = pnlPct
            };

            // Graficul investitiei: pornim din punctul de cumparare (data + pretul platit),
            // deci linia incepe exact pe linia orizontala de break-even,
            // apoi preturile de inchidere din zilele urmatoare, pana azi.
            dto.ChartDates.Add(r.BuyDate);
            dto.ChartPrices.Add(r.BuyPrice);

            foreach (var (date, close) in series.Between(r.BuyDate.AddDays(1), today))
            {
                dto.ChartDates.Add(date);
                dto.ChartPrices.Add(close);
            }

            return dto;
        }

        // Pentru fiecare zi de la prima investitie pana azi:
        //   valoare = suma( cantitate * pretul din ziua respectiva ) pentru investitiile deja facute
        //   cost    = suma( cantitate * pretul de cumparare )        pentru investitiile deja facute
        // In zilele fara pret (weekend la actiuni) folosim ultimul pret cunoscut.
        private static void BuildPortfolioChart(
            List<InvestmentRow> rows, Dictionary<int, PriceSeries> series, DateOnly today, PortfolioSummaryDto summary)
        {
            var start = rows.Min(r => r.BuyDate);

            for (var day = start; day <= today; day = day.AddDays(1))
            {
                decimal value = 0, cost = 0;

                foreach (var r in rows)
                {
                    if (r.BuyDate > day)
                        continue;   // investitia nu exista inca in ziua asta

                    cost += r.Quantity * r.BuyPrice;

                    // In ziua cumpararii, valoarea = suma platita (la fel ca graficul investitiei)
                    var price = day == r.BuyDate
                        ? r.BuyPrice
                        : series[r.AssetId].AtOrBefore(day) ?? r.BuyPrice;

                    value += r.Quantity * price;
                }

                summary.ChartDates.Add(day);
                summary.ChartValues.Add(Math.Round(value, 2));
                summary.ChartCosts.Add(Math.Round(cost, 2));
            }
        }

        // ---------------------------------------------------------------
        // Tipuri ajutatoare
        // ---------------------------------------------------------------

        private sealed class InvestmentRow
        {
            public int Id { get; set; }
            public int AssetId { get; set; }
            public string AssetCode { get; set; } = null!;
            public string AssetName { get; set; } = null!;
            public decimal? LastPrice { get; set; }
            public DateTime? LastPriceAt { get; set; }
            public decimal Quantity { get; set; }
            public decimal BuyPrice { get; set; }
            public DateOnly BuyDate { get; set; }
        }

        // Preturile zilnice ale unui asset, sortate dupa data.
        private sealed class PriceSeries
        {
            private readonly SortedList<DateOnly, decimal> _points = new();

            public PriceSeries(IEnumerable<(DateOnly Date, decimal Close)> points)
            {
                foreach (var (date, close) in points)
                    _points[date] = close;
            }

            public void Set(DateOnly date, decimal close) => _points[date] = close;

            public IEnumerable<(DateOnly Date, decimal Close)> Between(DateOnly from, DateOnly to)
                => _points.Where(p => p.Key >= from && p.Key <= to).Select(p => (p.Key, p.Value));

            // Pretul din ziua respectiva sau, daca lipseste, ultimul pret de dinainte.
            public decimal? AtOrBefore(DateOnly date)
            {
                var keys = _points.Keys;
                int lo = 0, hi = keys.Count - 1, found = -1;
                while (lo <= hi)                      // cautare binara
                {
                    var mid = (lo + hi) / 2;
                    if (keys[mid] <= date) { found = mid; lo = mid + 1; }
                    else hi = mid - 1;
                }
                return found >= 0 ? _points.Values[found] : null;
            }
        }
    }
}
