using System.Data;
using AccountyMinAPI.DB;
namespace AccountyMinAPI.Repositories;

public class PaymentTypeRepository : IPaymentTypeRepository
{
    private readonly ISqlDataAccess _db;
    private readonly ILoggerFactory _logger;

    public PaymentTypeRepository(ISqlDataAccess db, ILoggerFactory logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<(int, string)>> GetAllPaymentTypes()
    {
        var results = await _db.LoadData<(int, string), dynamic>(CommandType.StoredProcedure,
            "dbo.spPayments_GetAll", new {});
        return results;
    }

    public async Task InsertPaymentType(string categoryName) =>
        await _db.SaveData(CommandType.StoredProcedure, "dbo.spPayments_Insert", new 
        {
            categoryName
        });

    public async Task DeletePaymentTypeById(int id) =>
        await _db.SaveData(CommandType.StoredProcedure, "dbo.spPayments_Delete", new { Id = id });
}