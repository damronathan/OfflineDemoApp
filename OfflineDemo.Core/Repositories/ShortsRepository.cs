using OfflineDemo.Data;
using OfflineDemo.Models;
using OfflineDemo.Models.Dto;

namespace OfflineDemo.Core.Repositories;

public interface IShortsRepository
{
    Task<int> CreateShortAsync(CreateShortDto request);
    Task<List<ShortModel>> GetAllShortsAsync();
    Task UpdateShortAsync(UpdateShortDto request);
}

public class ShortsRepository : IShortsRepository
{
    private readonly ISqlDataAccess _sql;

    public ShortsRepository(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public async Task<List<ShortModel>> GetAllShortsAsync()
    {
        var shorts = await _sql.LoadData<ShortModel, dynamic>("spShorts_GetAll", new { }, "sql");
        return shorts;
    }
    public async Task<int> CreateShortAsync(CreateShortDto request)
    {
        int id = await _sql.SaveDataScalar<int, dynamic>("spShorts_CreateNew", request, "sql");
        return id;
    }
    public async Task UpdateShortAsync(UpdateShortDto request)
    {
        await _sql.SaveData("spShorts_AddUploadedFiles", request, "sql");
    }
}
