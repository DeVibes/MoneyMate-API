namespace AccountyMinAPI.Repositories;
public interface IUsernameRepository
{
    public Task<bool> IsUsernameAllowed(string username);
}