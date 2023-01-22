namespace AccountyMinAPI.Repositories;

public interface ITransactionRepository
{
    Task<(long, IEnumerable<TransactionModel>)> GetAllTransactions(TransactionsFilters filters);
    Task<TransactionModel> GetTransactionById(string id);
    Task<TransactionModel> InsertTransaction(TransactionModel transaction);
    Task DeleteTransactionById(string transactionId);
    Task<TransactionModel> EditTransaction(string transactionId, TransactionModel model);
    Task<IEnumerable<MonthlyCategorySummaryModel>> GetMonthlySummary(TransactionsFilters filters);
    Task<IEnumerable<YearlySummaryModel>> GetYearlySumByMonth(TransactionsFilters filters);
    Task<BalanceModel> GetMonthlyBalance(TransactionsFilters filters);
}