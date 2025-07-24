using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace Zeev.Zas.SpreadsheetReader.Service;

public class GoogleSheetsService
{
    private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
    private readonly string _appName;
    private readonly string _sheetId;
    private readonly string _worksheetName;
    private readonly string _credentialsPath;

    public GoogleSheetsService(string appName, string sheetId, string worksheetName, string credentialsPath)
    {
        _appName = appName ?? throw new ArgumentNullException(nameof(appName));
        _sheetId = sheetId ?? throw new ArgumentNullException(nameof(sheetId));
        _worksheetName = worksheetName ?? throw new ArgumentNullException(nameof(worksheetName));
        _credentialsPath = credentialsPath ?? throw new ArgumentNullException(nameof(credentialsPath));
    }

    public async Task<List<Dictionary<string, object?>>> GetSheetDataAsync()
    {
        var service = await InitializeGoogleSheetsApiAsync();
        var values = ReadRawSheetValues(service);

        if (values is null || values.Count == 0)
            throw new InvalidOperationException("No data found in the spreadsheet.");

        return ConvertToDictionaryList(values);
    }

    public async Task<List<Dictionary<string, object?>>> QuerySheetDataAsync(string key, string value)
    {
        var service = await InitializeGoogleSheetsApiAsync();
        var values = ReadRawSheetValues(service);

        if (values is null || values.Count == 0)
            throw new InvalidOperationException("No data found in the spreadsheet.");

        var dictionaryList = ConvertToDictionaryList(values);
        var filtered = dictionaryList
            .Where(item => item.TryGetValue(key, out var fieldValue) && fieldValue?.ToString()?.Contains(value, StringComparison.OrdinalIgnoreCase) == true)
            .ToList();

        return filtered;
    }

    private async Task<SheetsService> InitializeGoogleSheetsApiAsync()
    {
        if (!File.Exists(_credentialsPath))
            throw new FileNotFoundException("Credential file not found.", _credentialsPath);

        using var stream = new FileStream(_credentialsPath, FileMode.Open, FileAccess.Read);
        var credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);

        return new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = _appName
        });
    }

    private IList<IList<object>> ReadRawSheetValues(SheetsService service)
    {
        var range = $"{_worksheetName}!A:F";
        var request = service.Spreadsheets.Values.Get(_sheetId, range);
        var response = request.Execute();
        return response.Values;
    }

    private List<Dictionary<string, object?>> ConvertToDictionaryList(IList<IList<object>> values)
    {
        var headers = values[0];
        var data = new List<Dictionary<string, object?>>();

        for (int i = 1; i < values.Count; i++)
        {
            var row = values[i];
            var rowDict = new Dictionary<string, object?>();

            for (int j = 0; j < headers.Count; j++)
            {
                var key = headers[j]?.ToString() ?? $"Column{j + 1}";
                var value = j < row.Count ? row[j] : null;
                rowDict[key] = value;
            }

            data.Add(rowDict);
        }

        return data;
    }
}
