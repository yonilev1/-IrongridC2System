using AssetsApi.Dtos;
using AssetsApi.Models;
using AssetsApi.Services;
using Microsoft.AspNetCore.Mvc;
namespace AssetsApi.Controllers;

[ApiController]
[Route("api/reports")]
public class OperationsReportsController : ControllerBase
{
    private readonly IReportService _repository;
    public OperationsReportsController(IReportService repository)
    {
        _repository = repository;
    }

    [HttpGet("critical-assets")]
    public async Task<ActionResult<IEnumerable<AssetWithStatus>>> GetCriticalAssets()
    {
        return Ok(await _repository.GetCriticalAssets());
    }

    [HttpGet("unit/{unitId}/assets")]
    public async Task<ActionResult<IEnumerable<AssetsStatusPerUnit>?>> GetAllAssetsStatusOfEveryUnit(int unitId)
    {
        var data = await _repository.GetAllAssetsStatusOfEveryUnit(unitId);

        if (data == null)
            return NotFound();
        return Ok(data);
    }

}
