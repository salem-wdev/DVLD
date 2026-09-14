using DVLD.Core.Common;
using DVLD.Core.Enums;
using DVLD.Core.Interfaces;
using DVLD.Core.Settings;
using Microsoft.Extensions.Options;

namespace DVLD.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _basePath;

        public FileStorageService(IOptions<FileStorageSettings> options)
        {
            if (options?.Value == null || string.IsNullOrWhiteSpace(options.Value.BasePath))
                throw new InvalidOperationException("Storage BasePath is not configured in appsettings.json.");

            _basePath = options.Value.BasePath;

            CreateFolderIfDoesNotExist(_basePath);
        }

        public async Task<Result<string>> CopyFileToDestinationFolderWithGUIDAsync(string sourceFile, StorageFolder subFolder)
        {
            if (string.IsNullOrWhiteSpace(sourceFile))
                return Result<string>.Failure("Source file path cannot be empty.");

            if (!File.Exists(sourceFile))
                return Result<string>.Failure("Source file does not exist.");

            var targetDirectory = Path.Combine(_basePath, subFolder.ToString());

            if (!CreateFolderIfDoesNotExist(targetDirectory))
                return Result<string>.Failure("Failed to create the target directory.");

            string destinationFileName = GenerateUniqueFileName(sourceFile);
            string destinationFile = Path.Combine(targetDirectory, destinationFileName);

            try
            {
                await using var sourceStream = new FileStream(
                    sourceFile,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: 4096,
                    useAsync: true);

                await using var destinationStream = new FileStream(
                    destinationFile,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 4096,
                    useAsync: true);

                await sourceStream.CopyToAsync(destinationStream).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                DeleteFile(destinationFile);
                return Result<string>.Failure($"I/O error occurred while copying the file: {ex.Message}");
            }

            return Result<string>.Success(destinationFile);
        }

        public bool DeleteFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;

            try
            {
                File.Delete(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsFileExists(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            return File.Exists(filePath);
        }

        private static bool CreateFolderIfDoesNotExist(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                return false;

            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GenerateUniqueFileName(string sourceFile)
        {
            string extension = Path.GetExtension(sourceFile);
            return $"{Guid.NewGuid()}{extension}";
        }
    }
}