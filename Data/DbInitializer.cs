using Investment_Tracker.Entities;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;

namespace Investment_Tracker.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            await db.Database.MigrateAsync();

            if (await db.Assets.AnyAsync())
                return;

            var assets = new List<Asset>()
            {
                new() {code = "BTC",     name = "Bitcoin",             category = "Crypto"},
                new() {code = "ETH",     name = "Ethereum",            category = "Crypto"},
                                                                        
                new() {code = "SP500",   name = "S&P 500",             category = "Index"},
                new() {code = "NAS100",  name = "NASDAQ 100",          category = "Index"},
                                                                        
                new() {code = "NVDA",    name = "NVIDIA",              category = "Stock"},
                new() {code = "AAPL",    name = "Apple",               category = "Stock"},
                new() {code = "GOOGL",   name = "Alphabet (Google)",   category = "Stock"},
            };

            db.Assets.AddRange(assets);
            await db.SaveChangesAsync();
        }
    }
}
