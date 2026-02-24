using Investment_Tracker.Data;
using Investment_Tracker.Dtos;
using Investment_Tracker.Entities;
using Investment_Tracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

namespace Investment_Tracker.Controllers;

[ApiController]
[Route("api/investments")]
public class InvestmentsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly PortfolioService _portfolio;

    public InvestmentsController(AppDbContext db, PortfolioService portfolio)
    {
        _db = db;
        _portfolio = portfolio;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvestmentRequest request)
    {
        var assetExists = await _db.Assets.AnyAsync(a => a.id == request.assetId);
        if (!assetExists)
            return BadRequest("Invalid assetId");

        if (request.quantity <= 0)
            return BadRequest("quantity must be greater than 0");

        if (request.buyPrice <= 0)
            return BadRequest("buyPrice must be greater than 0");

        var investment = new Investment
        {
            assetId = request.assetId,
            quantity = request.quantity,
            buyPrice = request.buyPrice,
            buyDate = request.buyDate
        };

        _db.Investments.Add(investment);
        await _db.SaveChangesAsync();

        var created = await _db.Investments
            .Include(i => i.asset)
            .Where(i => i.id == investment.id)
            .Select(i => new
            {
                i.id,
                Asset = i.asset.code,
                i.quantity,
                i.buyPrice,
                i.buyDate,
                CostBasis = i.quantity * i.buyPrice
            })
            .SingleAsync();

        return Ok(created);
    }

    [HttpGet]
    public async Task<ActionResult<List<InvestmentResponseDto>>> GetAll()
    {
        var result = await _portfolio.GetInvestmentsWithPnlAsync();
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var inv = await _db.Investments.FindAsync(id);
        if (inv is null)
            return NotFound();

        _db.Investments.Remove(inv);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
