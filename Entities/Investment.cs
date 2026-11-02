namespace Investment_Tracker.Entities
{
    //1 Investment = o pozitie a unui utilizator legata de un asset
    public class Investment
    {
        public int id { get; set; }

        public int assetId { get; set; }
        public Asset asset { get; set; } = null!;

        public decimal quantity { get; set; }
        public decimal buyPrice { get; set; }

        public DateOnly buyDate { get; set; }

        public string userId { get; set; } = string.Empty;
    }
}
