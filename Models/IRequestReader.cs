namespace AccountyMinAPI.Models;

public interface IRequestReader<T>
{
    public T ReadFiltersFromRequest(HttpRequest request);
}