namespace Investment_Tracker.Dtos
{
    public class PortfolioSummaryDto
    {
        public decimal TotalCostBasis { get; set; }
        public decimal TotalCurrentValue { get; set; }
        public decimal TotalPnlUsd { get; set; }
        public decimal TotalPnlPct { get; set; }

        public List<DateOnly> ChartDates { get; set; } = new();
        public List<decimal> ChartValues { get; set; } = new();   
        public List<decimal> ChartCosts { get; set; } = new();    
    }
}
