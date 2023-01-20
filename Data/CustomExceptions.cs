namespace AccountyMinAPI.Data;

// public class NotFoundException : Exception
// {
//     public NotFoundException() {}
//     public NotFoundException(string message) : base(message)
//     {
//     }
// }

public class CategoryAlreadyExistsException : Exception
{
    public CategoryAlreadyExistsException(string categoryName) : base($"Category already exists - {categoryName}") {}
}

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException(string id) : base($"Category not found - {id}") {}
}

public class PaymentAlreadyExistsException : Exception
{
    public PaymentAlreadyExistsException(string paymentTypeName) : base($"Payment type already exists - {paymentTypeName}") {}
}

public class PaymentNotFoundException : Exception
{
    public PaymentNotFoundException(string id) : base($"Payment type not found - {id}") {}
}

public class AccountAlreadyExistsException : Exception
{
    public AccountAlreadyExistsException(string accountName) : base($"Account already exists - {accountName}") {}
}

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(string id) : base($"Account not found - {id}") {}
}


public class UserException : Exception
{
    public UserException(string errorMsg) : base(errorMsg) {}
}

public class ServerException : Exception
{
    public ServerException(string errorMsg) : base(errorMsg) {}
}

public class AlreadyExistsException : UserException
{
    public AlreadyExistsException(string errorMsg) : base(errorMsg) {}
}

public class NotFoundException : UserException
{
    public NotFoundException(string errorMsg) : base(errorMsg) {}
}

public class RequestException : UserException
{
    public RequestException(string errorMsg) : base(errorMsg) {}
}

/*

Bad Request - id not found, input already exists, input not found
Server error - db error / parsing error

*/