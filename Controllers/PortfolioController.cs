using Investment_Tracker.Data;
using Investment_Tracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace Investment_Tracker.Controllers;

[ApiController]
[Route("api/portfolio")]
public class PortfolioController : ControllerBase
{
    private readonly PortfolioService _portfolio;

    public PortfolioController(PortfolioService portfolio)
    {
        _portfolio = portfolio;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var userId = Request.Headers["userId"].FirstOrDefault();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("userId header missing");

        var summary = await _portfolio.GetSummaryAsync(userId);
        return Ok(summary);
    }
}
