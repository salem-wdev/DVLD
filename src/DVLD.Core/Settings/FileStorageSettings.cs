namespace DVLD.Core.Settings
{
    public class FileStorageSettings
    {
        public const string SectionName = "FileStorageSettings";

        public string BasePath { get; set; } = string.Empty;
        public string PeopleImagesFolder { get; set; } = "People";
        public string LicensesFolder { get; set; } = "Licenses";
    }
}