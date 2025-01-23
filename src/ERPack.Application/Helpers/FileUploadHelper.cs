using ERPack.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ERPack.Helpers;

public class FileUploadHelper : ERPackAppServiceBase, IFileUploadHelper
{
    private readonly string _baseDirectory;

    public FileUploadHelper()
    {
        _baseDirectory = ERPackConsts.BaseDirectory;
    }

    public async Task<string> SaveFileAsync(FileUpload fileUpload)
    {
        if (fileUpload.File == null || fileUpload.File.Length == 0)
            throw new ArgumentException("File cannot be null or empty", nameof(fileUpload.File));

        string fileName = $"{fileUpload.Id}_{fileUpload.Number}{Path.GetExtension(fileUpload.File.FileName)}";
        string directoryPath = Path.Combine(_baseDirectory, fileUpload.FolderName);
        string filePath = Path.Combine(directoryPath, fileName);

        EnsureDirectoryExists(directoryPath);
        DeleteExistingFiles(directoryPath, fileUpload.Id, fileUpload.Number);

        await SaveFileToPath(fileUpload.File, filePath);

        return Path.Combine(@"\uploads", fileUpload.FolderName, fileName);
    }

    private static void EnsureDirectoryExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    private static void DeleteExistingFiles(string directoryPath, string id, string number)
    {
        string searchPattern = $"{id}_{number}.*";
        var existingFiles = Directory.GetFiles(directoryPath, searchPattern);

        foreach (var existingFile in existingFiles)
        {
            File.Delete(existingFile);
        }
    }

    private async static Task SaveFileToPath(IFormFile file, string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await file.CopyToAsync(stream);
    }
}
