using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace OfflineDemo.Data;

public class SqlDataAccess : ISqlDataAccess
{
	private readonly IConfiguration _config;

	public SqlDataAccess(IConfiguration config)
	{
		_config = config;
	}

	public async Task<List<T>> LoadData<T, U>(string storedProcedure,
											  U parameters,
											  string connectionStringName)
	{
        using var connection = CreateConnection(connectionStringName);

        List<T> output = (await connection.QueryAsync<T>(
			storedProcedure,
			parameters,
			commandType: CommandType.StoredProcedure)).ToList();

		return output;
	}

	//Returns an id
	public async Task<T> SaveDataScalar<T, U>(string storedProcedure,
										   U parameters,
										   string connectionStringName)
	{
        using var connection = CreateConnection(connectionStringName);

        T? output = await connection.ExecuteScalarAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

		return output ?? throw new ArgumentNullException("The return value was null, which is an invalid result.");
	}

	//No return
	public async Task SaveData<T>(string storedProcedure,
								  T parameters,
								  string connectionStringName)
	{
		using var connection = CreateConnection(connectionStringName);

		await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
	}

	private IDbConnection CreateConnection(string connectionStringName)	
	{
        string connectionString = _config.GetConnectionString(connectionStringName) ??
            throw new KeyNotFoundException("Did not find the connection string specified");

        IDbConnection connection = new SqlConnection(connectionString);
		return connection;
    }
}
