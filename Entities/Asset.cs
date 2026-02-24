namespace Investment_Tracker.Entities
{
    public class Asset
    {
        public int id { get; set; }
        public string code { get; set; } = null!;     //BTC, GOOGL, AAPL etc.
        public string name { get; set; } = null!;
        public string category { get; set; } = null!; //Crypto, stock, index
    }
}
