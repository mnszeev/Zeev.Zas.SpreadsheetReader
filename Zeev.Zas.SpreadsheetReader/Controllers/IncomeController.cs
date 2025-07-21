using Microsoft.AspNetCore.Mvc;

namespace Zeev.Zas.SpreadsheetReader.Controllers;

[Route("income")]
[ApiController]
public class IncomeController : GoogleSheetsController
{
    private readonly IConfiguration _configuration;

    public IncomeController(IConfiguration configuration) : base(configuration) { }

    [HttpGet("pricing")]
    public async Task<IActionResult> GetPricing() => await GetSheetDataAsync("Income:Pricing");

    [HttpGet("human-resources")]
    public async Task<IActionResult> GetHumanResources() => await GetSheetDataAsync("Income:HumanResources");

    [HttpGet("daily/brazil")]
    public async Task<IActionResult> GetBrazilDaily() => await GetSheetDataAsync("Income:Daily:Brazil");

    [HttpGet("daily/foreign")]
    public async Task<IActionResult> GetForeignDaily() => await GetSheetDataAsync("Income:Daily:Foreign");
}
