namespace AccountyMinAPI.Repositories;

public interface IPaymentTypeRepository
{
    Task<RepositporyModel> GetAllPaymentTypes(string accountId);
    Task<RepositporyModel> InsertPaymentType(PaymentModel payment);
    Task<RepositporyModel> DeletePaymentTypeById(string accountId, string paymentId);
}