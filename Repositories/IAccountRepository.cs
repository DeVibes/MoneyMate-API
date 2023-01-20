namespace AccountyMinAPI.Repositories;

public interface IAccountRepository
{
    Task<AccountModel> InsertAccount(AccountModel account);
    Task<IEnumerable<AccountModel>> GetAllAccounts();
    Task<AccountModel> GetAccountById(string accountId);
    Task DeleteAccountById(string accountId);
    Task<AccountModel> AssignUserToAccount(string accountId, string userId);
    Task<AccountModel> DeassignUserToAccount(string accountId, string userId);
    Task<AccountModel> AssignPaymentTypeToAccount(string accountId, string paymentTypeId);
    Task<AccountModel> DeassignPaymentTypeToAccount(string accountId, string paymentTypeId);
    Task<AccountModel> AddAccountCategory(string accountId, CategoryModel model);
    Task<AccountModel> RemoveAccountCategory(string accountId, CategoryModel model);
    Task<AccountModel> EditAccount(string accountId, AccountModel accountModel);
    Task ValidateTransactionData(string accountId, TransactionModel model);
    // Task<(long, IEnumerable<TransactionModel>)> GetAllTransactions(TransactionsFilters filters);
    // Task<TransactionModel> GetTransactionById(string id);
    // Task DeleteTransactionById(string id);
    // Task UpdateTransactionById(string id, TransactionModel transaction);
    // Task PatchTransaction(string id, TransactionModel transaction);
    // Task PatchSeenStatus(string id, bool seen);
    // Task<BalanceModel> GetMonthlyBalance(TransactionsFilters filters);
    // Task<IEnumerable<TransactionCategoryModel>> GetMonthlyByCategory(TransactionsFilters filters);
    // Task<IEnumerable<TransactionMonthModel>> GetYearlySumByMonth(TransactionsFilters filters);
}