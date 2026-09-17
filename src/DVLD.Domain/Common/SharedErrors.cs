namespace DVLD.Domain.Common
{
    public static class SharedErrors
    {
        public static Error NotFound<T>(string detail = "") =>
            new Error($"{typeof(T).Name}.NotFound",
                      $"The {typeof(T).Name} was not found. {detail}".Trim());

        public static Error AddFailed<T>(string detail = "") =>
            new Error($"{typeof(T).Name}.AddFailed",
                      $"Failed to add the {typeof(T).Name}. {detail}".Trim());

        public static Error UpdateFailed<T>(string detail = "") =>
            new Error($"{typeof(T).Name}.UpdateFailed",
                      $"Failed to update the {typeof(T).Name}. {detail}".Trim());

        public static Error DeleteFailed<T>(string detail = "") =>
            new Error($"{typeof(T).Name}.DeleteFailed",
                      $"Failed to delete the {typeof(T).Name}. {detail}".Trim());

        public static Error InvalidInput<T>(string detail) =>
            new Error($"{typeof(T).Name}.InvalidInput", detail);

        public static Error Conflict<T>(string detail) =>
            new Error($"{typeof(T).Name}.Conflict", detail);
    }
}