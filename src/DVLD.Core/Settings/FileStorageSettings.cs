using DVLD.Core.Enums;

namespace DVLD.Core.Settings
{
    public class FileStorageSettings
    {
        public const string SectionName = "FileStorageSettings";

        public string BasePath { get; set; } = string.Empty;
    }
}