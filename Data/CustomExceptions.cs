namespace AccountyMinAPI.Data;

public class UserException : SystemException
{
    public UserException(string errorMsg) : base(errorMsg) {}
}

public class ServerException : SystemException
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