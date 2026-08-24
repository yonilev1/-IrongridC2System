using Microsoft.AspNetCore.Mvc;
using AssetsApi.Services;
using AssetsApi.Models;
namespace AssetsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController :ControllerBase
{
    private readonly IAssetService _repository;

    public AssetsController(IAssetService repository)
    {
        _repository = repository;
    }

    [HttpGet("id")]
    public async Task<ActionResult<AssetsEvent>> GetAssetById(int id)
    {
        var asset = await _repository.GetAssetById(id);
        if (asset == null)
            return NotFound();
        return Ok(asset);
    }

    [HttpPost("units")]
    public async Task<IActionResult> Create(UnitsEvent unit)
    {
        bool created = await _repository.CreateUnit(unit);
        if (created == true)
        {
            return CreatedAtAction(nameof(Create), unit);
        }
        
        return BadRequest();
    }
}
