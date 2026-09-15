namespace DVLD.Infrastructure.Settings
{
    public class FileStorageSettings
    {
        public const string SectionName = "FileStorageSettings";

        public string BasePath { get; set; } = string.Empty;
    }
}