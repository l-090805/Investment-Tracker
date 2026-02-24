namespace Investment_Tracker.Dtos
{
    public class InvestmentResponseDto
    {
        public int Id { get; set; }
        public string AssetCode { get; set; } = null!;

        public decimal Quantity { get; set; }
        public decimal BuyPrice { get; set; }
        public DateOnly BuyDate { get; set; }

        public decimal CurrentPrice { get; set; }
        public decimal CostBasis { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal PnlUsd { get; set; }
        public decimal PnlPct { get; set; }
    }
}
