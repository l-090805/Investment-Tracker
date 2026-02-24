namespace Investment_Tracker.Services;

public static class AssetSymbolMapper
{
    public static string ToMarketSymbol(string assetCode)
    {
        return assetCode.ToUpperInvariant() switch
        {
            "BTC"    =>  "BTC/USD",
            "ETH"    =>  "ETH/USD",
            "SP500"  =>  "SPY",
            "NAS100" =>  "QQQ",
            _        =>  assetCode.ToUpperInvariant()
            //using ETF prices for S&p500 & Nas100
        };
    }
}
