namespace AccountyMinAPI.Models;

public class TransactionsFilters
{
    public int? CategoryId { get; set; }
    public int? PaymentTypeId { get; set; }
    public bool? Seen { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public static TransactionsFilters ReadFiltersFromQuery(HttpRequest request)
    {
        var isCatValid = int.TryParse(request.Query["categoryId"], out int categoryId);
        var isPaymentValid = int.TryParse(request.Query["paymentId"], out int paymentId);
        var isSeenValid = bool.TryParse(request.Query["seen"], out bool seen);
        var isStartDateValid = DateTime.TryParse(request.Query["startDate"], out DateTime startDate);
        var isEndDateValid = DateTime.TryParse(request.Query["endDate"], out DateTime endDate);
        return new()
        {
            CategoryId = isCatValid ? categoryId : null,
            PaymentTypeId = isPaymentValid ? paymentId : null,
            Seen = isSeenValid ? seen : null,
            StartDate = isStartDateValid ? startDate: null,
            EndDate = isEndDateValid ? endDate: null,
        };
    }
}