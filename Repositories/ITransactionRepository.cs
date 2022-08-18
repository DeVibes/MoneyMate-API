namespace AccountyMinAPI.Repositories;

public interface ITransactionRepository
{
    Task<IEnumerable<TransactionModel>> GetAllTransactions(TransactionsFilters filters);
    Task<TransactionModel> GetTransactionById(int id);
    Task InsertTransaction(TransactionModel transaction);
    Task DeleteTransactionById(int id);
    Task UpdateTransactionById(int id, TransactionModel transaction);
    Task PatchTransaction(int id, TransactionModel transaction);
}