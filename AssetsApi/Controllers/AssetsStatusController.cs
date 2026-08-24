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
}
