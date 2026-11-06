using System.Globalization;
using System.Text.Json;

namespace Investment_Tracker.Services;

public class TwelveDataPriceService
{
    private const string BaseUrl = "https://api.twelvedata.com";

    private readonly HttpClient _http;
    private readonly string _apiKey;

    public TwelveDataPriceService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["MarketData:TwelveDataApiKey"]
            ?? throw new InvalidOperationException("Missing MarketData:TwelveDataApiKey in configuration.");
    }

    public async Task<Dictionary<string, decimal>> GetPricesAsync(IReadOnlyCollection<string> symbols)
    {
        var result = new Dictionary<string, decimal>();
        if (symbols.Count == 0)
            return result;

        var joined = string.Join(",", symbols.Select(Uri.EscapeDataString));
        var url = $"{BaseUrl}/price?symbol={joined}&apikey={_apiKey}";

        using var doc = JsonDocument.Parse(await _http.GetStringAsync(url));
        var root = doc.RootElement;

        if (symbols.Count == 1)
        {
            // Pentru un singur simbol raspunsul este: { "price": "123.45" }
            if (TryReadPrice(root, out var price))
                result[symbols.First()] = price;
            return result;
        }

        // Pentru mai multe simboluri: { "AAPL": { "price": "..." }, "BTC/USD": { "price": "..." } }
        foreach (var symbol in symbols)
        {
            if (root.TryGetProperty(symbol, out var item) && TryReadPrice(item, out var price))
                result[symbol] = price;
        }

        return result;
    }

    // Istoricul zilnic (pret de inchidere) de la 'from' pana azi. 1 credit / request.
    public async Task<List<(DateOnly Date, decimal Close)>> GetDailyHistoryAsync(string symbol, DateOnly from)
    {
        var url = $"{BaseUrl}/time_series?symbol={Uri.EscapeDataString(symbol)}" +
                  $"&interval=1day&start_date={from:yyyy-MM-dd}&outputsize=5000&apikey={_apiKey}";

        using var doc = JsonDocument.Parse(await _http.GetStringAsync(url));
        var root = doc.RootElement;

        if (!root.TryGetProperty("values", out var values) || values.ValueKind != JsonValueKind.Array)
        {
            var message = root.TryGetProperty("message", out var m) ? m.GetString() : "no values";
            throw new InvalidOperationException($"Twelve Data time_series failed for {symbol}: {message}");
        }

        var points = new List<(DateOnly, decimal)>();
        foreach (var v in values.EnumerateArray())
        {
            var dateText = v.GetProperty("datetime").GetString();
            var closeText = v.GetProperty("close").GetString();

            // "datetime" poate fi "2026-11-05" sau "2026-11-05 00:00:00" -> pastram doar data
            if (dateText is null || closeText is null || dateText.Length < 10) continue;
            if (!DateOnly.TryParseExact(dateText[..10], "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var date)) continue;
            if (!decimal.TryParse(closeText, NumberStyles.Any, CultureInfo.InvariantCulture, out var close)) continue;

            points.Add((date, close));
        }

        return points.OrderBy(p => p.Item1).ToList();
    }

    private static bool TryReadPrice(JsonElement element, out decimal price)
    {
        price = 0;
        return element.ValueKind == JsonValueKind.Object
            && element.TryGetProperty("price", out var p)
            && decimal.TryParse(p.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out price);
    }
}
