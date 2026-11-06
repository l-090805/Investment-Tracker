namespace Investment_Tracker.Entities
{
    // Un punct pe grafic = pretul de inchidere al unui asset intr-o zi
    public class PriceHistory
    {
        public int id { get; set; }

        public int assetId { get; set; }
        public Asset asset { get; set; } = null!;

        public DateOnly date { get; set; }
        public decimal close { get; set; }
    }
}
