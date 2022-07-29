using AccountyMinAPI.Models;

namespace AccountyMinAPI.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionModel>> GetAllTransactions();
        Task InsertTransaction(TransactionModel transaction);
    }
}