namespace AccountyMinAPI.Models;

public record TransactionsFilters
{
    public string? Category { get; set; } 
    public string? PaymentType { get; set; } 
    public int PageNumber { get; set; }
    public bool? Seen { get; set; }
    public DateTime FromDate { get; set; } = DateTime.UtcNow.AddYears(-1);
    public DateTime ToDate { get; set; } = DateTime.UtcNow;
    public IEnumerable<string> Users { get; set; } = Enumerable.Empty<string>();
    public static TransactionsFilters ReadFiltersFromQuery(HttpRequest request)
    {
        string category = request.Query["category"];
        string paymentType = request.Query["payment"];
        var isPageValid = int.TryParse(request.Query["page"], out int pageNumber);
        var isSeenValid = bool.TryParse(request.Query["seen"], out bool seen);
        var isStartDateValid = DateTime.TryParse(request.Query["fromDate"], out DateTime fromDate);
        if (!isStartDateValid)
            throw new RequestException("Invalid start date");
        var isEndDateValid = DateTime.TryParse(request.Query["toDate"], out DateTime toDate);
        if (!isEndDateValid)
            throw new RequestException("Invalid end date");
        return new()
        {
            Category = category,
            PaymentType = paymentType,
            PageNumber = isPageValid ? pageNumber : 0,
            Seen = isSeenValid ? seen : null,
            FromDate = fromDate,
            ToDate = toDate
        };
    }
}