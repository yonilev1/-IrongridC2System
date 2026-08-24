using AssetsApi.Dtos;
using AssetsApi.Models;
using AssetsApi.Services;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("{id}")]
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsset(int id, UpdateAsset asset)
    {
        var updated = await _repository.UpdateAsset(id, asset);
        if (updated)
            return NoContent();
        return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(int id)
    {
        var deleted = await _repository.DeleteAsset(id);

        if (deleted)
            return NoContent();
        return NotFound();
    }

}
