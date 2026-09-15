namespace DVLD.Core.Common
{
    /// <summary>
    /// Represents the outcome of an operation that does not return data.
    /// Encapsulates execution status and error details to eliminate exception-driven control flow.
    /// </summary>

    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? Error { get; }

        /// <summary>
        /// Enforces domain invariants to guarantee mutual exclusivity between success and failure states.
        /// </summary>
        protected Result(bool isSuccess, string? error)
        {
            if (isSuccess && !string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException("A successful result cannot contain an error message.");

            if (!isSuccess && string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException("A failure result must specify an error message.");

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Creates a successful operation outcome.
        /// </summary>
        public static Result Success() => new(true, null);

        /// <summary>
        /// Creates a failed operation outcome with a specific error description.
        /// </summary>
        public static Result Failure(string error) => new(false, error);
    }
}
