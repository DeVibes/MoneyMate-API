using System.Data;
using System.Data.SqlClient;
using AccountyMinAPI.Data;
using Dapper;

namespace AccountyMinAPI.DB
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            this._config = config;
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(
            CommandType commandType,
            string storedProcedureOrQuery,
            U parameters,
            string connectionId = "Default")
        {
            // "using" keyword for closing connection after reaching end of scope
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            var rows = await connection.QueryAsync<T>(storedProcedureOrQuery, parameters, commandType: commandType);
            return rows;
        }

        public async Task SaveData<T>(
            CommandType commandType,
            string storedProcedureOrQuery,
            T parameters,
            string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            var result = await connection.ExecuteAsync(storedProcedureOrQuery, parameters, commandType: commandType);
        }
    }
}