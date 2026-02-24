using Investment_Tracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Investment_Tracker.Controllers;

[ApiController]
[Route("api/assets")]
public class AssetsController : ControllerBase
{
    private readonly AppDbContext _db;

    public AssetsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var assets = await _db.Assets
            .OrderBy(a => a.category)
            .ThenBy(a => a.code)
            .Select(a => new
            {
                a.id,
                a.code,
                a.name,
                a.category
            })
            .ToListAsync();

        return Ok(assets);
    }
}
