using Investment_Tracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace Investment_Tracker.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            await db.Database.MigrateAsync();

            var renames = new Dictionary<string, string>
            {
                ["SP500"] = "SPY",
                ["NAS100"] = "QQQ",
            };

            foreach (var (oldCode, newCode) in renames)
            {
                var old = await db.Assets.FirstOrDefaultAsync(a => a.code == oldCode);
                var alreadyNew = await db.Assets.AnyAsync(a => a.code == newCode);

                if (old != null && !alreadyNew)
                    old.code = newCode;
            }
            await db.SaveChangesAsync();

            var wanted = new List<Asset>
            {
                // Crypto
                new() { code = "BTC",  name = "Bitcoin",  category = "Crypto", symbol = "BTC/USD" },
                new() { code = "ETH",  name = "Ethereum", category = "Crypto", symbol = "ETH/USD" },
                new() { code = "SOL",  name = "Solana",   category = "Crypto", symbol = "SOL/USD" },
                new() { code = "XRP",  name = "XRP",      category = "Crypto", symbol = "XRP/USD" },
                new() { code = "DOGE", name = "Dogecoin", category = "Crypto", symbol = "DOGE/USD" },

                // ETF-uri 
                new() { code = "SPY", name = "SPDR S&P 500 ETF",             category = "ETF", symbol = "SPY" },
                new() { code = "QQQ", name = "Invesco QQQ (NASDAQ 100 ETF)", category = "ETF", symbol = "QQQ" },
                new() { code = "DIA", name = "SPDR Dow Jones ETF",           category = "ETF", symbol = "DIA" },
                new() { code = "IWM", name = "iShares Russell 2000 ETF",     category = "ETF", symbol = "IWM" },
                new() { code = "GLD", name = "SPDR Gold Shares",             category = "ETF", symbol = "GLD" },
                new() { code = "SLV", name = "iShares Silver Trust",         category = "ETF", symbol = "SLV" },
                new() { code = "USO", name = "United States Oil Fund",       category = "ETF", symbol = "USO" },

                // Acțiuni individuale
                new() { code = "NVDA",  name = "NVIDIA",            category = "Stock", symbol = "NVDA" },
                new() { code = "AAPL",  name = "Apple",             category = "Stock", symbol = "AAPL" },
                new() { code = "GOOGL", name = "Alphabet (Google)", category = "Stock", symbol = "GOOGL" },
                new() { code = "MSFT",  name = "Microsoft",         category = "Stock", symbol = "MSFT" },
                new() { code = "AMZN",  name = "Amazon",            category = "Stock", symbol = "AMZN" },
                new() { code = "TSLA",  name = "Tesla",             category = "Stock", symbol = "TSLA" },
                new() { code = "META",  name = "Meta",              category = "Stock", symbol = "META" },
                new() { code = "NFLX",  name = "Netflix",           category = "Stock", symbol = "NFLX" },
                new() { code = "JPM",   name = "JPMorgan Chase",    category = "Stock", symbol = "JPM" },
                new() { code = "KO",    name = "Coca-Cola",         category = "Stock", symbol = "KO" },
            };

            var existing = await db.Assets.ToDictionaryAsync(a => a.code);

            foreach (var w in wanted)
            {
                if (existing.TryGetValue(w.code, out var e))
                {
                    e.name = w.name;
                    e.category = w.category;
                    e.symbol = w.symbol;
                }
                else
                {
                    db.Assets.Add(w);
                }
            }

            await db.SaveChangesAsync();
        }
    }
}
