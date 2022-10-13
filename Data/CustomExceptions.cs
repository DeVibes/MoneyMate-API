namespace AccountyMinAPI.Data;

public class NotFoundException : Exception
{
    public NotFoundException() {}
    public NotFoundException(string message) : base(message)
    {
    }
}

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