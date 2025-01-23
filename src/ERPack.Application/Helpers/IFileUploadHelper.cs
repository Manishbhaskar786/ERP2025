using ERPack.Shared;
using System.Threading.Tasks;

namespace ERPack.Helpers;

public interface IFileUploadHelper
{
    public Task<string> SaveFileAsync(FileUpload file);
}
