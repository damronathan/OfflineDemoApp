﻿

namespace OfflineDemo.Data
{
	public interface ISqlDataAccess
	{
		Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
		Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
		Task<T> SaveDataScalar<T, U>(string storedProcedure, U parameters, string connectionStringName);
	}
}