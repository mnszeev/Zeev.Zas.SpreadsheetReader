using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;
using Zeev.Zas.SpreadsheetReader.Dtos;
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

            var parsed = await service.GetSheetDataAsync();
            var json = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });
            var result = Regex.Unescape(json);
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

    protected async Task<IActionResult> GetPagedSheetDatasync(string sectionKey, int pageNumber, int pageSize)
    {
        try
        {
            var service = new GoogleSheetsService(
                appName: _configuration[$"GoogleSheets:{sectionKey}:AppName"],
                sheetId: _configuration[$"GoogleSheets:{sectionKey}:SheetId"],
                worksheetName: _configuration[$"GoogleSheets:{sectionKey}:WorksheetName"],
                credentialsPath: _configuration["GoogleSheets:CredentialsPath"]);

            var allRows = await service.GetSheetDataAsync();
            var totalRecords = allRows.Count();
            var pagedData = allRows
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<Dictionary<string, object>>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = pagedData
            };

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

    protected async Task<IActionResult> GetSheetDataWithQueryAsync(string sectionKey)
    {
        try
        {
            var service = new GoogleSheetsService(
                appName: _configuration[$"GoogleSheets:{sectionKey}:AppName"],
                sheetId: _configuration[$"GoogleSheets:{sectionKey}:SheetId"],
                worksheetName: _configuration[$"GoogleSheets:{sectionKey}:WorksheetName"],
                credentialsPath: _configuration["GoogleSheets:CredentialsPath"]);

            var parsed = await service.GetSheetDataAsync();
            var json = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });
            var result = Regex.Unescape(json);
            return Ok(result.AsQueryable());
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

    protected async Task<IActionResult> QuerySheetDataAsync(string sectionKey, string key, string value)
    {
        try
        {
            var service = new GoogleSheetsService(
                appName: _configuration[$"GoogleSheets:{sectionKey}:AppName"],
                sheetId: _configuration[$"GoogleSheets:{sectionKey}:SheetId"],
                worksheetName: _configuration[$"GoogleSheets:{sectionKey}:WorksheetName"],
                credentialsPath: _configuration["GoogleSheets:CredentialsPath"]);

            var parsed = await service.QuerySheetDataAsync(key, value);
            var json = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });
            var result = Regex.Unescape(json);
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
