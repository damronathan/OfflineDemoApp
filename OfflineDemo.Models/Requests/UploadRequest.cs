using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OfflineDemo.Models.Requests;

public class UploadRequest
{
    [Required]
    public string Title { get; set; }

    public string? Description { get; set; }

    public string? Hashtags { get; set; }

    [Required]
    public IFormFile Mp4File { get; set; }

    [Required]
    public IFormFile ImageFile { get; set; }
}
