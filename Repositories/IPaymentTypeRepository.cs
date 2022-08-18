namespace AccountyMinAPI.Repositories;

public interface IPaymentTypeRepository
{
    Task<IEnumerable<(int, string)>> GetAllPaymentTypes();
    Task InsertPaymentType(string paymentTypeName);
    Task DeletePaymentTypeById(int id);
}