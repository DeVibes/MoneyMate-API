namespace AccountyMinAPI.Models;
public record BalanceModel
{
    public double Income { get; init; }
    public double Outcomes { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
}