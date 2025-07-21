using Microsoft.AspNetCore.Mvc;
using Zeev.Zas.SpreadsheetReader.Service;

namespace Zeev.Zas.SpreadsheetReader.Controllers;

public class GoogleSheetsController : ControllerBase
{
    protected readonly IConfiguration _configuration;

    protected GoogleSheetsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected async Task<IActionResult> GetSheetDataAsync(string sectionKey)
    {
        try
        {
            var service = new GoogleSheetsService(
                appName: _configuration[$"GoogleSheets:{sectionKey}:AppName"],
                sheetId: _configuration[$"GoogleSheets:{sectionKey}:SheetId"],
                worksheetName: _configuration[$"GoogleSheets:{sectionKey}:WorksheetName"],
                credentialsPath: _configuration["GoogleSheets:CredentialsPath"]);

            var result = await service.GetSheetDataAsJsonAsync();
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao acessar a planilha: {ex.Message}");
        }
    }
}
