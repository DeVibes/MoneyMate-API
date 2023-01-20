namespace AccountyMinAPI.Repositories;
public record RepositporyModel
{
    public string ErrorMessage { get; set; } = String.Empty;
    public ErrorTypeEnum ErrorType { get; set; } = ErrorTypeEnum.NONE;
    public dynamic Payload { get; set; } 
}

public enum ErrorTypeEnum
{
    NONE,
    ACCOUNT_NOT_FOUND,
    ALREADY_EXISTS,
    USER_ALREADY_ASSIGNED,
    USER_NOT_ASSIGNED,
    PAYMENT_ALEADY_ASSIGNED,
    PAYMENT_NOT_ASSIGNED,
    DB_ERROR
}