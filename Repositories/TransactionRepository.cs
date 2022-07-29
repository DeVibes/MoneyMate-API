using AccountyMinAPI.DB;
using AccountyMinAPI.Models;

namespace AccountyMinAPI.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ISqlDataAccess _db;

        public TransactionRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<TransactionModel>> GetAllTransactions() =>
            _db.LoadData<TransactionModel, dynamic>("dbo.spTransactions_GetAll", new { });

        public async Task InsertTransaction(TransactionModel transaction) =>
            await _db.SaveData("dbo.spTransactions_Insert", new 
            {
                transaction.CategoryId,
                Date = transaction.Date.ToString("o"),
                transaction.Description,
                transaction.Seen,
                transaction.PaymentTypeId,
                transaction.Price,
                transaction.Store
            });
    }
}