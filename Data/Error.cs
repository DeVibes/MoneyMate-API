namespace AccountyMinAPI.Data;

public enum MappingResponse
{
    OK,
    MISSING_PRICE,
    MISSING_DATE,
    MISSING_CATEGORY,
    MISSING_PAYMENT,
    CATEGORY_NOT_EXISTS,
    PAYMENT_NOT_EXISTS,
    INVALID_DATE
}