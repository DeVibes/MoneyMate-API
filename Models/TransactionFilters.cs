namespace AccountyMinAPI.Models;

public class TransactionsFilters
{
    public int? CategoryId { get; set; }
    public int? PaymentTypeId { get; set; }
    public int PageNumber { get; set; }
    public bool? Seen { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public static TransactionsFilters ReadFiltersFromQuery(HttpRequest request)
    {
        var isPageValid = int.TryParse(request.Query["page"], out int pageNumber);
        var isCatValid = int.TryParse(request.Query["categoryId"], out int categoryId);
        var isPaymentValid = int.TryParse(request.Query["paymentId"], out int paymentId);
        var isSeenValid = bool.TryParse(request.Query["seen"], out bool seen);
        var isStartDateValid = DateTime.TryParse(request.Query["fromDate"], out DateTime fromDate);
        var isEndDateValid = DateTime.TryParse(request.Query["toDate"], out DateTime toDate);
        return new()
        {
            CategoryId = isCatValid ? categoryId : null,
            PaymentTypeId = isPaymentValid ? paymentId : null,
            Seen = isSeenValid ? seen : null,
            FromDate = isStartDateValid ? fromDate : null,
            ToDate = isEndDateValid ? toDate : null,
            PageNumber = isPageValid ? pageNumber : 0
        };
    }
}