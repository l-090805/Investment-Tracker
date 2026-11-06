namespace Investment_Tracker.Entities
{
    public class Asset
    {
        public int id { get; set; }
        public string code { get; set; } = null!;     //BTC, GOOGL, AAPL etc.
        public string name { get; set; } = null!;
        public string category { get; set; } = null!; //Crypto, ETF, Stock..
        public string symbol { get; set; } = null!;   //simbolul Twelve Data

        // ultimul pret cunoscut, actualizat de job 
        public decimal? lastPrice { get; set; }
        public DateTime? lastPriceAt { get; set; }     //UTC

        //
        // De la ce data avem istoric descarcat pentru acest asset.
        public DateOnly? historyFrom { get; set; }
        // Ultima zi in care am sincronizat istoricul (max o data pe zi)
        public DateOnly? historySyncedOn { get; set; }
    }
}
