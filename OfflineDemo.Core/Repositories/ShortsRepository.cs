using OfflineDemo.Data;
using OfflineDemo.Models;

namespace OfflineDemo.Core.Repositories;

public class ShortsRepository
{
    private readonly ISqlDataAccess _sql;

    public ShortsRepository(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public async Task<List<ShortsModel>> GetAllShortsAsync()
    {
        return await _sql.LoadData<ShortsModel, dynamic>("spShorts_GetAll", new { }, "sql");
    }
}
