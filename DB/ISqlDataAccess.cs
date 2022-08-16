using System.Data;

namespace AccountyMinAPI.DB
{

    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(CommandType commandType, string storedProcedure, U parameters, string connectionId = "Default");
        Task SaveData<T>(CommandType commandType, string storedProcedure, T parameters, string connectionId = "Default");
    }
}