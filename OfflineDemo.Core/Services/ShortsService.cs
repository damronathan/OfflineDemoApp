using OfflineDemo.Models;
using OfflineDemo.Core.Repositories;
using OfflineDemo.Models.Requests;
using OfflineDemo.Models.Dto;

namespace OfflineDemo.Core.Services;

public interface IShortsService
{
    Task<IEnumerable<ShortModel>> GetAllShorts();
    Task UploadShort(UploadRequest request);
}
public class ShortsService : IShortsService

{
    private readonly IShortsRepository _repo;
    private readonly IStorageService _storageService;

    public ShortsService(IShortsRepository repo, IStorageService storageService)
    {
        _repo = repo;
        _storageService = storageService;
    }
    public async Task<IEnumerable<ShortModel>> GetAllShorts()
    {
        return await _repo.GetAllShortsAsync();
    }

    public async Task UploadShort(UploadRequest request)
    {
        if (request.Mp4File == null || request.ImageFile == null)
        {
            throw new ArgumentException("Both MP4 and image files are required.");
        }

        // Validate file size
        const long maxFileSize = 800L * 1024 * 1024; // 800MB
        if (request.Mp4File.Length > maxFileSize)
        {
            throw new ArgumentException("MP4 file exceeds the 800MB limit.");
        }
        var createShortRequest = new CreateShortDto
        {
            Title = request.Title,
            Description = request.Description,
            Hashtags = request.Hashtags
        };
        
        int id = await _repo.CreateShortAsync(createShortRequest);
        var mp4FileUrl = await _storageService.UploadFile("shorts", id, request.Mp4File);
        var imageFileUrl = await _storageService.UploadFile("images", id, request.ImageFile);
        var updateShortRequest = new UpdateShortDto
        {
            Id = id,
            Mp4FileUrl = mp4FileUrl,
            ImageFileUrl = imageFileUrl
        };

        await _repo.UpdateShortAsync(updateShortRequest);
    }
}

