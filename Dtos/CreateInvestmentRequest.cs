namespace Investment_Tracker.Dtos;

public class CreateInvestmentRequest
{
    public int assetId { get; set; }
    public decimal quantity { get; set; }
    public decimal buyPrice { get; set; }
    public DateOnly buyDate { get; set; }
}
