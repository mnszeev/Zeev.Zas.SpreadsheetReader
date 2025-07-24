using Microsoft.AspNetCore.Mvc;
using Zeev.Zas.SpreadsheetReader.Dtos;

namespace Zeev.Zas.SpreadsheetReader.Controllers;

[Route("educational")]
[ApiController]
public class EducationalController : GoogleSheetsController
{
    private readonly IConfiguration _configuration;

    public EducationalController(IConfiguration configuration) : base(configuration) { }

    [HttpGet("pricing")]
    public async Task<IActionResult> GetPricing(int pageNumber = 1, int pageSize = 50) => await GetPagedSheetDatasync("Educational:Pricing", pageNumber, pageSize);

    [HttpPost("pricing")]
    public async Task<IActionResult> QueryPricing([FromBody] QueryRequest dto) => await QuerySheetDataAsync("Educational:Pricing", dto.Key, dto.Value);

    [HttpGet("human-resources")]
    public async Task<IActionResult> GetHumanResources(int pageNumber = 1, int pageSize = 50) => await GetPagedSheetDatasync("Educational:HumanResources", pageNumber, pageSize);

    [HttpPost("human-resources")]
    public async Task<IActionResult> QueryHumanResources([FromBody] QueryRequest dto) => await QuerySheetDataAsync("Educational:HumanResources", dto.Key, dto.Value);

    [HttpGet("daily/brazil")]
    public async Task<IActionResult> GetBrazilDaily(int pageNumber = 1, int pageSize = 50) => await GetPagedSheetDatasync("Educational:Daily:Brazil", pageNumber, pageSize);

    [HttpPost("daily/brazil")]
    public async Task<IActionResult> QueryBrazilDaily([FromBody] QueryRequest dto) => await QuerySheetDataAsync("Educational:Daily:Brazil", dto.Key, dto.Value);

    [HttpGet("daily/foreign")]
    public async Task<IActionResult> GetForeignDaily(int pageNumber = 1, int pageSize = 50) => await GetPagedSheetDatasync("Educational:Daily:Foreign", pageNumber, pageSize);

    [HttpPost("daily/foreign")]
    public async Task<IActionResult> QueryForeignDaily([FromBody] QueryRequest dto) => await QuerySheetDataAsync("Educational:Daily:Foreign", dto.Key, dto.Value);
}
