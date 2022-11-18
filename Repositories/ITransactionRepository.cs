namespace AccountyMinAPI.Repositories;

public interface ITransactionRepository
{
    Task<(long, IEnumerable<TransactionModel>)> GetAllTransactions(TransactionsFilters filters);
    Task<TransactionModel> GetTransactionById(string id);
    Task InsertTransaction(TransactionModel transaction);
    Task DeleteTransactionById(string id);
    Task UpdateTransactionById(string id, TransactionModel transaction);
    Task PatchTransaction(string id, TransactionModel transaction);
    Task PatchSeenStatus(string id, bool seen);
}