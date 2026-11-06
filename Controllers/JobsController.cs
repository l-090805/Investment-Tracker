using Investment_Tracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace Investment_Tracker.Controllers;

//Apelat de cron-job.org o data la 15 min

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly PriceRefreshService _refresh;
    private readonly IConfiguration _config;

    public JobsController(PriceRefreshService refresh, IConfiguration config)
    {
        _refresh = refresh;
        _config = config;
    }

    [HttpPost("refresh-prices")]
    public async Task<IActionResult> RefreshPrices()
    {
        var expectedKey = _config["Jobs:Key"];
        var providedKey = Request.Headers["X-Job-Key"].FirstOrDefault();

        if (string.IsNullOrEmpty(expectedKey) || providedKey != expectedKey)
            return Unauthorized("invalid job key");

        var result = await _refresh.RefreshHeldAssetsAsync();
        return Ok(result);
    }
}
