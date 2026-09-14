using DVLD.Core.Common;
namespace DVLD.Core.Interfaces
{
    public interface IFileStorageService
    {
        bool CreateFolderIfDoesNotExist(string FolderPath);
        string ReplaceFileNameWithGUID(string sourceFile);
        Task<Result<string>> CopyFileToDestinationFolderWithGUIDAsync(string sourceFile, string destinationFolder);
        bool DeleteFile(string sourceFile);
        bool IsFileExists(string sourceFile);
    }
}
