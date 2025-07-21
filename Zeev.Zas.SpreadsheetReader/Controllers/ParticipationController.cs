using Microsoft.AspNetCore.Mvc;

namespace Zeev.Zas.SpreadsheetReader.Controllers;

[Route("participation")]
[ApiController]
public class ParticipationController : GoogleSheetsController
{
    private readonly IConfiguration _configuration;

    public ParticipationController(IConfiguration configuration) : base(configuration) { }

    [HttpGet("pricing")]
    public async Task<IActionResult> GetPricing() => await GetSheetDataAsync("Educational:Pricing");

    [HttpGet("human-resources")]
    public async Task<IActionResult> GetHumanResources() => await GetSheetDataAsync("Educational:HumanResources");

    [HttpGet("daily/brazil")]
    public async Task<IActionResult> GetBrazilDaily() => await GetSheetDataAsync("Educational:Daily:Brazil");

    [HttpGet("daily/foreign")]
    public async Task<IActionResult> GetForeignDaily() => await GetSheetDataAsync("Educational:Daily:Foreign");
}
