using Microsoft.AspNetCore.Http;

namespace ERPack.Shared;

public class FileUpload
{
    public string Number { get; set; }
    public string Id { get; set; }
    public string FolderName { get; set; }
    public IFormFile File { get; set; }
}

