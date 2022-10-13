namespace AccountyMinAPI.Repositories;

public interface IPaymentTypeRepository
{
    Task<IEnumerable<PaymentModel>> GetAllPaymentTypes();
    Task InsertPaymentType(PaymentModel payment);
    Task<bool> DeletePaymentTypeById(string id);
}