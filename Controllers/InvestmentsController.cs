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
    private readonly PriceRefreshService _prices;
    private readonly ILogger<InvestmentsController> _logger;

    public InvestmentsController(
        AppDbContext db,
        PortfolioService portfolio,
        PriceRefreshService prices,
        ILogger<InvestmentsController> logger)
    {
        _db = db;
        _portfolio = portfolio;
        _prices = prices;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvestmentRequest request)
    {
        var userId = Request.Headers["userId"].FirstOrDefault();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("userId header missing");

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
            buyDate = request.buyDate,
            userId = userId
        };

        _db.Investments.Add(investment);
        await _db.SaveChangesAsync();

        //aducem istoricul de la data cumpararii (1 credit, doar daca lipseste),
        // ca graficul si pretul curent sa fie disponibile imediat.
        try
        {
            await _prices.EnsureHistoryForInvestmentAsync(investment.assetId, investment.buyDate);
        }
        catch (Exception ex)
        {
            // Investitia e deja salvata, o eroare nu o anuleaza
            _logger.LogWarning(ex, "History backfill failed for investment {Id}", investment.id);
        }

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
        var userId = Request.Headers["userId"].FirstOrDefault();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("userId header missing");

        var result = await _portfolio.GetInvestmentsWithPnlAsync(userId);
        return Ok(new { investments = result });
    }

    //o singura investitie, pentru pagina de view details / investitie
    [HttpGet("{id:int}")]
    public async Task<ActionResult<InvestmentResponseDto>> GetById(int id)
    {
        var userId = Request.Headers["userId"].FirstOrDefault();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("userId header missing");

        var investment = await _portfolio.GetInvestmentAsync(userId, id);
        if (investment is null)
            return NotFound();

        return Ok(investment);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = Request.Headers["userId"].FirstOrDefault();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("userId header missing");

        var inv = await _db.Investments.FindAsync(id);
        if (inv is null)
            return NotFound();

        if (inv.userId != userId)
            return Forbid();

        _db.Investments.Remove(inv);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}