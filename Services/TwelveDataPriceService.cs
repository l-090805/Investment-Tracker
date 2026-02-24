using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;

namespace Investment_Tracker.Services;

public class TwelveDataPriceService
{
    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;
    private readonly string _apiKey;

    public TwelveDataPriceService(HttpClient http, IMemoryCache cache, IConfiguration config)
    {
        _http = http;
        _cache = cache;
        _apiKey = config["MarketData:TwelveDataApiKey"]
            ?? throw new InvalidOperationException("Missing MarketData:TwelveDataApiKey in configuration.");
    }

    public async Task<decimal> GetCurrentPriceAsync(string symbol)
    {
        // cache 60s per symbol (reduce drastic calls)
        var cacheKey = $"td:price:{symbol}";
        if (_cache.TryGetValue(cacheKey, out decimal cached))
            return cached;

        var url = $"https://api.twelvedata.com/price?symbol={Uri.EscapeDataString(symbol)}&apikey={_apiKey}";

        var resp = await _http.GetFromJsonAsync<PriceResponse>(url);
        if (resp is null || string.IsNullOrWhiteSpace(resp.Price))
            throw new InvalidOperationException($"No price returned for symbol {symbol}");

        if (!decimal.TryParse(resp.Price, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var price))
            throw new InvalidOperationException($"Invalid price format for {symbol}: {resp.Price}");

        _cache.Set(cacheKey, price, TimeSpan.FromSeconds(60));
        return price;
    }

    private sealed class PriceResponse
    {
        [JsonPropertyName("price")]
        public string? Price { get; set; }
    }
}
