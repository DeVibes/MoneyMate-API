namespace AccountyMinAPI.Repositories;

public interface IAccountRepository
{
    Task<AccountModel> InsertAccount(AccountModel account);
    Task<IEnumerable<AccountModel>> GetAllAccounts();
    Task<AccountModel> GetAccountById(string accountId);
    Task DeleteAccountById(string accountId);
    Task<AccountModel> AssignUserToAccount(string accountId, UserModel user);
    Task<AccountModel> DeassignUserToAccount(string accountId, UserModel user);
    Task<AccountModel> AssignPaymentTypeToAccount(string accountId, string paymentTypeId);
    Task<AccountModel> DeassignPaymentTypeToAccount(string accountId, string paymentTypeId);
    Task<AccountModel> AddAccountCategory(string accountId, string category);
    Task<AccountModel> RemoveAccountCategory(string accountId, string category);
    Task<AccountModel> EditAccount(string accountId, AccountModel accountModel);
    Task ValidateTransactionData(string accountId, TransactionModel model);
    Task<IEnumerable<UserModel>> GetAccountUsers(string accountId);
    Task<Tuple<string, string>> GetUserAccountAndRole(string username);
}