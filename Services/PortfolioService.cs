using Investment_Tracker.Data;
using Investment_Tracker.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Investment_Tracker.Services
{
    public class PortfolioService
    {
        private readonly AppDbContext _db;
        private readonly TwelveDataPriceService _price;

        public PortfolioService(AppDbContext db, TwelveDataPriceService price)
        {
            _db = db;
            _price = price;
        }

        public async Task<List<InvestmentResponseDto>> GetInvestmentsWithPnlAsync(string userId)
        {
            var investments = await _db.Investments
                .Include(i => i.asset)
                .Where(i => i.userId == userId)
                .Select(i => new
                {
                    i.id,
                    AssetCode = i.asset.code,
                    Symbol = i.asset.symbol,
                    i.quantity,
                    i.buyPrice,
                    i.buyDate
                })
                .ToListAsync();

            if (investments.Count == 0)
                return new List<InvestmentResponseDto>();


            var distinctAssets = investments
             .Select(i => new { i.AssetCode, i.Symbol })
             .Distinct()
             .ToList();

            var currentPrices = new Dictionary<string, decimal>();

            foreach (var a in distinctAssets)
            {
                try
                {
                    currentPrices[a.AssetCode] = await _price.GetCurrentPriceAsync(a.Symbol);
                }
                catch
                {
                    currentPrices[a.AssetCode] = 0m;
                }
            }

            var result = investments.Select(i =>
            {
                var currentPrice = currentPrices[i.AssetCode];

                var costBasis = i.quantity * i.buyPrice;
                var currentValue = i.quantity * currentPrice;

                var pnlUsd = currentValue - costBasis;
                var pnlPct = costBasis == 0 ? 0 : (pnlUsd / costBasis) * 100;

                return new InvestmentResponseDto
                {
                    Id = i.id,
                    AssetCode = i.AssetCode,
                    Quantity = i.quantity,
                    BuyPrice = i.buyPrice,
                    BuyDate = i.buyDate,
                    CurrentPrice = currentPrice,
                    CostBasis = costBasis,
                    CurrentValue = currentValue,
                    PnlUsd = pnlUsd,
                    PnlPct = pnlPct
                };
            }).ToList();

            return result;
        }

        public async Task<PortfolioSummaryDto> GetSummaryAsync(string userId)
        {
            var list = await GetInvestmentsWithPnlAsync(userId);

            if (list.Count == 0)
            {
                return new PortfolioSummaryDto
                {
                    TotalCostBasis = 0m,
                    TotalCurrentValue = 0m,
                    TotalPnlUsd = 0m,
                    TotalPnlPct = 0m
                };
            }

            var totalCostBasis = list.Sum(x => x.CostBasis);
            var totalCurrentValue = list.Sum(x => x.CurrentValue);
            var totalPnlUsd = totalCurrentValue - totalCostBasis;
            var totalPnlPct = totalCostBasis == 0 ? 0 : (totalPnlUsd / totalCostBasis) * 100;

            return new PortfolioSummaryDto
            {
                TotalCostBasis = totalCostBasis,
                TotalCurrentValue = totalCurrentValue,
                TotalPnlUsd = totalPnlUsd,
                TotalPnlPct = totalPnlPct
            };
        }
    }
}
