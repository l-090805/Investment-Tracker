using Investment_Tracker.Data;
using Investment_Tracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Investment_Tracker.Controllers;


[ApiController]
[Route("api/portfolio")]

public class PortfolioController :ControllerBase
{
    private readonly AppDbContext _db;
    private readonly TwelveDataPriceService _price;

    public PortfolioController(AppDbContext db, TwelveDataPriceService price)
    {
        _db = db;
        _price = price;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var investments = await _db.Investments
            .Include(i => i.asset)
            .Select(i => new
            {
                i.id,
                assetCode = i.asset.code,
                i.quantity,
                i.buyPrice,
            })
            .ToListAsync();

        if(investments.Count == 0)
        {
            return Ok(new
            {
                TotalCostBasis = 0m,
                TotalCurrentValue = 0m,
                TotalPnlUsd = 0m,
                TotalPnlPercentage = 0m
            });
        }

        var distinctAssetCodes = investments
            .Select(i => i.assetCode)
            .Distinct()
            .ToList();

        var currentPrices = new Dictionary<string, decimal>();

        foreach (var code in distinctAssetCodes)
        {
            try
            {
                var symbol = AssetSymbolMapper.ToMarketSymbol(code);
                var price = await _price.GetCurrentPriceAsync(symbol);
                currentPrices[code] = price;
            }
            catch
            {
                currentPrices[code] = 0m;
            }
        }

        decimal totalCostBasis = 0m;
        decimal totalCurrentValue = 0m;

        foreach(var i in investments)
        {
            var costBasis = i.quantity * i.buyPrice;
            var currentPrice = currentPrices[i.assetCode];
            var currentValue = i.quantity * currentPrice;

            totalCostBasis += costBasis;
            totalCurrentValue += currentValue;
        }

        var totalPnlUsd = totalCurrentValue - totalCostBasis;
        var totalPnlPercentage = totalCostBasis == 0 ? 0 : (totalPnlUsd / totalCostBasis) * 100;

        return Ok(new
        {
            TotalCostBasis = totalCostBasis,
            TotalCurrentValue = totalCurrentValue,
            TotalPnlUsd = totalPnlUsd,
            TotalPnlPercentage = totalPnlPercentage
        });
    }
}
