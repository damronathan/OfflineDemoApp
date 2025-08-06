using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OfflineDemo.Models.Config;

namespace OfflineDemo.Core.Services;

public interface IStorageService
{
    Task ValidateBlobContainer();
    Task<string> UploadFile(string folder, int id, IFormFile file);
}
public class StorageService : IStorageService
{
    private readonly BlobContainerClient _blobContainerClient;

    public StorageService(IOptions<AzureStorageConfig> configOptions)
    {
        var config = configOptions.Value;
        var blobServiceClient = new BlobServiceClient(config.ConnectionString);
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(config.ContainerName);
    }

    public async Task ValidateBlobContainer()
    {
        await _blobContainerClient.CreateIfNotExistsAsync();
        await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.None);
    }

    public async Task<string> UploadFile(string folder, int id, IFormFile file)
    {
        string fileName = folder switch
        {
            "shorts" => $"{id}.mp4",
            // TODO - Fix this extension lookup
            "images" => $"{id}.{file.FileName.Split('.')[1]}",
            _ => throw new ArgumentException("Invalid folder type.", nameof(folder))
        };
        
        var client = _blobContainerClient.GetBlobClient($"{folder}/{fileName}");
        await using (var stream = file.OpenReadStream())
        {
            try
            {
                await client.UploadAsync(stream, true);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        return client.Uri.ToString();
    }

}

