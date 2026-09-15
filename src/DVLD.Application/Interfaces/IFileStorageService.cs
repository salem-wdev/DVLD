using DVLD.Domain.Common;
using DVLD.Domain.Enums;

namespace DVLD.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<Result<string>> CopyFileToDestinationFolderWithGUIDAsync(string sourceFile, StorageFolder subFolderPath);
        bool DeleteFile(string sourceFile);
        bool IsFileExists(string sourceFile);
    }
}
