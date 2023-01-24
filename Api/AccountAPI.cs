using Microsoft.AspNetCore.Mvc;

namespace AccountyMinAPI.Api;
public static class AccountAPI
{
    public static async Task<IResult> CreateAccount([FromBody]AccountRequest requestBody, IAccountRepository repository)
    {
        try
        {
            AccountModel model = AccountRequest.ToAccountModelCreate(requestBody);
            AccountModel createdAccount = await repository.InsertAccount(model);
            AccountResponse payload = AccountModel.ToAccountResponse(createdAccount);
            return Results.Created(payload.Id, payload);
        }
        catch (UserException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> GetAccounts(IAccountRepository repository)
    {
        try
        {
            IEnumerable<AccountModel> accounts = await repository.GetAllAccounts();
            List<AccountResponse> payload = accounts.Select(AccountModel.ToAccountResponse).ToList();
            return Results.Ok(payload);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> DeleteAccountById(string accountId, IAccountRepository repository)
    {
        try
        {
            await repository.DeleteAccountById(accountId);
            return Results.Ok($"Account id '{accountId}' successfully deleted");
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> GetAccountById(string accountId, IAccountRepository repository)
    {
        try
        {
            AccountModel account = await repository.GetAccountById(accountId);
            AccountResponse payload = AccountModel.ToAccountResponse(account);
            return Results.Ok(payload);
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> AssignUserToAccount(string accountId, [FromBody]AccountUser requestBody, IAccountRepository repository)
    {
        try
        {
            if(string.IsNullOrEmpty(requestBody.Id))
                throw new RequestException("Missing user");
            AccountModel updatedAccount = await repository.AssignUserToAccount(accountId, requestBody.Id);
            AccountResponse payload = AccountModel.ToAccountResponse(updatedAccount);
            return Results.Ok(payload);
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (System.Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> DeassignUserFromAccount(string accountId, [FromBody]AccountUser requestBody, IAccountRepository repository)
    {
        try
        {
            if(string.IsNullOrEmpty(requestBody.Id))
                throw new RequestException("Missing user");
            AccountModel updatedAccount = await repository.DeassignUserToAccount(accountId, requestBody.Id);
            AccountResponse payload =  AccountModel.ToAccountResponse(updatedAccount);
            return Results.Ok(payload);
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (System.Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> AssignPaymentTypeToAccount(string accountId, [FromBody]AccountPaymentType requestBody, IAccountRepository repository)
    {
        try
        {
            if(string.IsNullOrEmpty(requestBody.Name))
                throw new RequestException("Missing payment type");
            AccountModel updatedAccount = await repository.AssignPaymentTypeToAccount(accountId, requestBody.Name);
            AccountResponse payload = AccountModel.ToAccountResponse(updatedAccount);
            return Results.Ok(payload);
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (System.Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> DeassignPaymentTypeFromAccount(string accountId, [FromBody]AccountPaymentType requestBody, IAccountRepository repository)
    {
        try
        {
            if(string.IsNullOrEmpty(requestBody.Name))
                throw new RequestException("Missing payment type");
            AccountModel updatedAccount = await repository.DeassignPaymentTypeToAccount(accountId, requestBody.Name);
            AccountResponse payload = AccountModel.ToAccountResponse(updatedAccount);
            return Results.Ok(payload);
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (System.Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> AddCategoryToAccount(string accountId, [FromBody]AccountCategory requestBody, IAccountRepository repository)
    {
        try
        {
            if(string.IsNullOrEmpty(requestBody.Name))
                throw new RequestException("Missing category");
            AccountModel updatedAccount = await repository.AddAccountCategory(accountId, requestBody.Name);
            AccountResponse responseModel = AccountModel.ToAccountResponse(updatedAccount);
            return Results.Ok(responseModel);
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (System.Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> RemoveCategoryFromAccount(string accountId, [FromBody]AccountCategory requestBody, IAccountRepository repository)
    {
        try
        {
            if(string.IsNullOrEmpty(requestBody.Name))
                throw new RequestException("Missing category");
            AccountModel updatedAccount = await repository.RemoveAccountCategory(accountId, requestBody.Name);
            AccountResponse payload = AccountModel.ToAccountResponse(updatedAccount);
            return Results.Ok(payload);
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (System.Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }

    public static async Task<IResult> PatchAccountById(string accountId, [FromBody]AccountRequest patchRequest, IAccountRepository repository)
    {
        try
        {
            AccountModel model = AccountRequest.ToAccountModelPatch(patchRequest);
            AccountModel updatedAccount = await repository.EditAccount(accountId, model);
            AccountResponse payload = AccountModel.ToAccountResponse(updatedAccount);
            return Results.Ok(payload);
        }
        catch (UserException ex)
        {
            if (ex is NotFoundException)
                return Results.NotFound(ex.Message);
            return Results.BadRequest(ex.Message);
        }
        catch (ServerException ex)
        {
            return Results.Problem(ex.Message);
        }
        catch (System.Exception)
        {
            return Results.Problem("Ops unexpeccted behavior");
        }
    }
}