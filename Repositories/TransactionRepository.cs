using System.Data;
using AccountyMinAPI.DB;
namespace AccountyMinAPI.Repositories;

// Id prop is a string in mongo
public class TransactionRepository
{
    private readonly ISqlDataAccess _db;
    private readonly ILoggerFactory _logger;

    public TransactionRepository(ISqlDataAccess db, ILoggerFactory logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<TransactionModel>> GetAllTransactions(TransactionsFilters filters) 
    {
        var results = await _db.LoadData<TransactionModel, dynamic>(CommandType.StoredProcedure, 
            "dbo.spTransactions_GetAll", filters);
        return results;
    }

    public async Task<TransactionModel> GetTransactionById(int id) 
    {
        var result = await _db.LoadData<TransactionModel, dynamic>(CommandType.StoredProcedure, "dbo.spTransactions_Get", new { Id = id });
        return result.FirstOrDefault();
    }

    public async Task InsertTransaction(TransactionModel transaction) =>
        await _db.SaveData(CommandType.StoredProcedure, "dbo.spTransactions_Insert", new 
        {
            transaction.CategoryId,
            transaction.Date,
            transaction.Description,
            transaction.Seen,
            transaction.PaymentTypeId,
            transaction.Price,
            transaction.Store
        });

    public async Task DeleteTransactionById(int id) =>
        await _db.SaveData(CommandType.StoredProcedure, "dbo.spTransactions_Delete", new { Id = id });

    public async Task UpdateTransactionById(int id, TransactionModel transaction)
    {
        await _db.SaveData(CommandType.StoredProcedure, "dbo.spTransactions_Update", new 
        {
            Id = id,
            transaction.CategoryId,
            transaction.Date,
            transaction.Description,
            transaction.Seen,
            transaction.PaymentTypeId,
            transaction.Price,
            transaction.Store
        });
    }

    public async Task PatchTransaction(int id, TransactionModel transaction)
    {
        await _db.SaveData(CommandType.StoredProcedure, "dbo.spTransactions_Patch", new
        {
            Id = id,
            transaction.CategoryId,
            transaction.Date,
            transaction.Description,
            transaction.Seen,
            transaction.PaymentTypeId,
            transaction.Price,
            transaction.Store
        });
    }
}