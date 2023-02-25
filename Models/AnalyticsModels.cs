namespace AccountyMinAPI.Models;

public record MonthlyCategorySummaryModel
{
    public string Category { get; set; } = String.Empty;
    public double Total { get; set; }
    public DateTime Date { get; set; }

    public static MonthlyCategorySummaryResponse ToResponse(MonthlyCategorySummaryModel model)
    {
        if (model is null)
            throw new ServerException($"Error in mapping {nameof(model)} to {nameof(MonthlyCategorySummaryResponse)}");
        return new()
        {
            Category = model.Category,
            Total = model.Total.ToString(),
            // DateString = model.Date.ToString("o")
        };
    }
}

public record MonthlyCategorySummaryResponse
{
    public string Category { get; set; } = String.Empty;
    public string Total { get; set; } = String.Empty;
    // public string DateString { get; set; } = String.Empty;
}

public record YearlySummaryModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public double Total { get; set; }
    public static YearlySummaryResponse ToResponse(YearlySummaryModel model)
    {
        if (model is null)
            throw new ServerException($"Error in mapping {nameof(model)} to {nameof(YearlySummaryResponse)}");
        return new()
        {
            Year = model.Year.ToString(),
            Month = model.Month.ToString(),
            Total = model.Total.ToString()
        };
    }
}

public record YearlySummaryResponse
{
    public string Year { get; set; } = String.Empty;
    public string Month { get; set; } = String.Empty;
    public string Total { get; set; } = String.Empty;
}

public record BalanceResponse
{
    public double Income { get; set; } = 0;
    public double Outcome { get; set; } = 0;
    public IEnumerable<MonthlyCategorySummaryModel> CategoriesDetails = Enumerable.Empty<MonthlyCategorySummaryModel>();
    public string FromDateString { get; set; } = String.Empty;
    public string ToDateString { get; set; } = String.Empty;
}