using DVLD.Core.Common;
using DVLD.Core.Enums;

namespace DVLD.Core.Interfaces
{
    public interface IFileStorageService
    {
        Task<Result<string>> CopyFileToDestinationFolderWithGUIDAsync(string sourceFile, StorageFolder subFolderPath);
        bool DeleteFile(string sourceFile);
        bool IsFileExists(string sourceFile);
    }
}
