using OfflineDemo.Data;
using OfflineDemo.Models;
using OfflineDemo.Core.Repositories;
using System.ComponentModel;

namespace OfflineDemo.Core.Services;

public interface IShortsService
{
    Task<IEnumerable<ShortsModel>> GetAllShorts();
}
public class ShortsService : IShortsService

{
    private readonly ShortsRepository _repo;

    public ShortsService(ShortsRepository repo)
    {
        _repo = repo;
    }
    public async Task<IEnumerable<ShortsModel>> GetAllShorts()
    {
        return await _repo.GetAllShortsAsync();
    }
}

