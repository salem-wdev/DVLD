namespace DVLD.Domain.Common
{
    public static class SharedErrors
    {
        // Base string-based methods
        public static Error NotFound(string entityName, string detail = "") =>
            new Error($"{entityName}.NotFound", $"The {entityName} was not found. {detail}".Trim());

        public static Error AddFailed(string entityName, string detail = "") =>
            new Error($"{entityName}.AddFailed", $"Failed to add the {entityName}. {detail}".Trim());

        public static Error UpdateFailed(string entityName, string detail = "") =>
            new Error($"{entityName}.UpdateFailed", $"Failed to update the {entityName}. {detail}".Trim());

        public static Error DeleteFailed(string entityName, string detail = "") =>
            new Error($"{entityName}.DeleteFailed", $"Failed to delete the {entityName}. {detail}".Trim());

        public static Error InvalidInput(string entityName, string detail) =>
            new Error($"{entityName}.InvalidInput", detail);

        public static Error Conflict(string entityName, string detail) =>
            new Error($"{entityName}.Conflict", detail);

        // Generic overloads delegating to the base methods
        public static Error NotFound<T>(string detail = "") =>
            NotFound(typeof(T).Name, detail);

        public static Error AddFailed<T>(string detail = "") =>
            AddFailed(typeof(T).Name, detail);

        public static Error UpdateFailed<T>(string detail = "") =>
            UpdateFailed(typeof(T).Name, detail);

        public static Error DeleteFailed<T>(string detail = "") =>
            DeleteFailed(typeof(T).Name, detail);

        public static Error InvalidInput<T>(string detail) =>
            InvalidInput(typeof(T).Name, detail);

        public static Error Conflict<T>(string detail) =>
            Conflict(typeof(T).Name, detail);
    }
}