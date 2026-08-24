using AssetsApi.Dtos;
using AssetsApi.Models;
using AssetsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AssetsApi.Controllers;

[ApiController]
[Route("api/assets-status")]
public class AssetsStatusController : ControllerBase
{
    private readonly IAssetService _repository;
    public AssetsStatusController(IAssetService repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssetsEvent>>> GetAllWithStatus()
    {
        return Ok(await _repository.GetAllWithStatus());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetsEvent?>> GetFullAssetWithStatus(int id)
    {
        var asset = await _repository.GetFullAssetWithStatus(id);
        if (asset == null)
            return NotFound();
        return Ok(asset);
    }

    [HttpGet("status")]
    public async Task<ActionResult<IEnumerable<AssetsEvent>>> GetAssetByStatus(string status)
    {
        return Ok(await _repository.GetAssetByStatus(status));
    }

}
