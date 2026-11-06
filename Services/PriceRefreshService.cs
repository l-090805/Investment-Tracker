using Investment_Tracker.Data;
using Investment_Tracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace Investment_Tracker.Services;

// Preturi de la twelve data aduse in db. Userul primeste pretul din db, nu mai face call uri.
public class PriceRefreshService
{
    //Twelve data free plan are 8 credite/min max
    public const int MaxCreditsPerRun = 8;

    private readonly AppDbContext _db;
    private readonly TwelveDataPriceService _twelveData;
    private readonly ILogger<PriceRefreshService> _logger;

    public PriceRefreshService(AppDbContext db, TwelveDataPriceService twelveData, ILogger<PriceRefreshService> logger)
    {
        _db = db;
        _twelveData = twelveData;
        _logger = logger;
    }

    // 1) sincronizeaza istoricul zilnic pentru asset-urile detinute (max o data pe zi / asset)
    // 2) actualizeaza pretul curent, incepand cu cele mai vechi preturi
    public async Task<RefreshResult> RefreshHeldAssetsAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var credits = MaxCreditsPerRun;
        var result = new RefreshResult();

        // Asset urile detinute = asset urile care apar in cel putin o investitie,
        // impreuna cu cea mai veche data de cumparare (de acolo incepe graficul).
        var investments = await _db.Investments
            .Select(i => new { i.assetId, i.buyDate })
            .ToListAsync();

        var held = investments
            .GroupBy(i => i.assetId)
            .Select(g => new { AssetId = g.Key, FirstBuyDate = g.Min(i => i.buyDate) })
            .ToList();

        if (held.Count == 0)
            return result;

        var heldIds = held.Select(h => h.AssetId).ToList();
        var assets = await _db.Assets.Where(a => heldIds.Contains(a.id)).ToListAsync();

        //Istoric zilnic
        foreach (var asset in assets)
        {
            if (credits == 0) break;

            var firstBuyDate = held.First(h => h.AssetId == asset.id).FirstBuyDate;
            if (!NeedsHistorySync(asset, firstBuyDate, today))
                continue;

            credits--;
            if (await SyncHistoryAsync(asset, firstBuyDate, today))
                result.HistorySynced.Add(asset.code);
            else
                result.Failed.Add(asset.code);
        }

        //Pret curent, intr-un singur request batch
        var toRefresh = assets
            .OrderBy(a => a.lastPriceAt ?? DateTime.MinValue)
            .Take(credits)
            .ToList();

        if (toRefresh.Count > 0)
        {
            try
            {
                var prices = await _twelveData.GetPricesAsync(toRefresh.Select(a => a.symbol).ToList());

                foreach (var asset in toRefresh)
                {
                    if (prices.TryGetValue(asset.symbol, out var price))
                    {
                        await SavePriceAsync(asset, price, today);
                        result.PricesUpdated.Add(asset.code);
                    }
                    else
                    {
                        result.Failed.Add(asset.code);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Batch price refresh failed");
                result.Failed.AddRange(toRefresh.Select(a => a.code));
            }
        }

        await _db.SaveChangesAsync();
        return result;
    }

    // Apelat la adaugarea unei investitii: daca nu avem istoric de la data cumpararii,
    // il descarcam acum (1 credit), ca graficul si pretul curent sa apara imediat.
    public async Task EnsureHistoryForInvestmentAsync(int assetId, DateOnly buyDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var asset = await _db.Assets.FindAsync(assetId);
        if (asset is null)
            return;

        var coveredFromBuyDate = asset.historyFrom is not null && asset.historyFrom <= buyDate;
        if (coveredFromBuyDate && asset.lastPrice is not null)
            return;

        if (await SyncHistoryAsync(asset, buyDate, today))
            await _db.SaveChangesAsync();
    }

    //daca un asset detinut nu are inca
    // niciun pret salvat  il aducem acum.
    public async Task EnsureLatestPricesAsync(IReadOnlyCollection<int> assetIds)
    {
        var ids = assetIds.ToList();
        var missing = await _db.Assets
            .Where(a => ids.Contains(a.id) && a.lastPrice == null)
            .Take(MaxCreditsPerRun)
            .ToListAsync();

        if (missing.Count == 0)
            return;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        try
        {
            var prices = await _twelveData.GetPricesAsync(missing.Select(a => a.symbol).ToList());
            foreach (var asset in missing)
            {
                if (prices.TryGetValue(asset.symbol, out var price))
                    await SavePriceAsync(asset, price, today);
            }
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fallback price fetch failed");
        }
    }

    // Avem nevoie de istoric daca: nu avem deloc, nu acopera prima cumparare,
    // sau nu l-am mai sincronizat azi (completam zilele lipsa).
    private static bool NeedsHistorySync(Asset asset, DateOnly firstBuyDate, DateOnly today)
    {
        if (asset.historyFrom is null || asset.historyFrom > firstBuyDate)
            return true;

        return asset.historySyncedOn is null || asset.historySyncedOn < today;
    }

    // Descarca istoricul zilnic si il salveaza (insert sau update pe zi).
    // Daca avem deja istoric de la data ceruta, descarcam doar de la ultima zi salvata.
    private async Task<bool> SyncHistoryAsync(Asset asset, DateOnly wantedFrom, DateOnly today)
    {
        var from = wantedFrom;
        if (asset.historyFrom is not null && asset.historyFrom <= wantedFrom)
        {
            var lastSaved = await _db.PriceHistory
                .Where(p => p.assetId == asset.id)
                .MaxAsync(p => (DateOnly?)p.date);

            if (lastSaved is not null)
                from = lastSaved.Value;
        }

        try
        {
            var points = await _twelveData.GetDailyHistoryAsync(asset.symbol, from);

            var existing = await _db.PriceHistory
                .Where(p => p.assetId == asset.id && p.date >= from)
                .ToDictionaryAsync(p => p.date);

            foreach (var (date, close) in points)
            {
                if (existing.TryGetValue(date, out var row))
                    row.close = close;
                else
                    _db.PriceHistory.Add(new PriceHistory { assetId = asset.id, date = date, close = close });
            }

            if (asset.historyFrom is null || wantedFrom < asset.historyFrom)
                asset.historyFrom = wantedFrom;
            asset.historySyncedOn = today;

            // Ultimul punct din istoric e un pret valid daca nu avem inca unul mai nou.
            if (points.Count > 0 && asset.lastPrice is null)
            {
                asset.lastPrice = points[^1].Close;
                asset.lastPriceAt = DateTime.UtcNow;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "History sync failed for {Symbol}", asset.symbol);
            return false;
        }
    }

    // Salveaza pretul curent pe asset si in randul zilei de azi din istoric.
    private async Task SavePriceAsync(Asset asset, decimal price, DateOnly today)
    {
        asset.lastPrice = price;
        asset.lastPriceAt = DateTime.UtcNow;

        // Cautam intai in ce am adaugat deja in aceasta rulare (inca nesalvat),
        // altfel am insera de doua ori aceeasi zi.
        var todayRow = _db.PriceHistory.Local
                           .FirstOrDefault(p => p.assetId == asset.id && p.date == today)
                       ?? await _db.PriceHistory
                           .FirstOrDefaultAsync(p => p.assetId == asset.id && p.date == today);

        if (todayRow is null)
            _db.PriceHistory.Add(new PriceHistory { assetId = asset.id, date = today, close = price });
        else
            todayRow.close = price;
    }
}

public class RefreshResult
{
    public List<string> PricesUpdated { get; } = new();
    public List<string> HistorySynced { get; } = new();
    public List<string> Failed { get; } = new();
}
