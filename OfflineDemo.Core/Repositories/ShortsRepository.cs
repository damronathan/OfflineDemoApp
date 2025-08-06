using OfflineDemo.Data;
using OfflineDemo.Models;

namespace OfflineDemo.Core.Repositories;

public interface IShortsRepository
{
    Task<int> CreateShortAsync(CreateShortRequest request);
    Task<List<ShortsModel>> GetAllShortsAsync();
    Task UpdateShortAsync(UpdateShortRequest request);
}

public class ShortsRepository : IShortsRepository
{
    private readonly ISqlDataAccess _sql;

    public ShortsRepository(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public async Task<List<ShortsModel>> GetAllShortsAsync()
    {
        var shorts = await _sql.LoadData<ShortsModel, dynamic>("spShorts_GetAll", new { }, "sql");
        return shorts;
    }
    public async Task<int> CreateShortAsync(CreateShortRequest request)
    {
        int id = await _sql.SaveDataScalar<int, dynamic>("spShorts_CreateNew", request, "sql");
        return id;
    }
    public async Task UpdateShortAsync(UpdateShortRequest request)
    {
        await _sql.SaveData("spShorts_AddUploadedFiles", request, "sql");
    }
}
