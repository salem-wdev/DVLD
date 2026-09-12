using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Core.Common
{
    /// <summary>
    /// Represents the outcome of an operation that returns a value on success.
    /// Provides fail-fast access to ensure callers inspect the operation status before accessing data.
    /// </summary>
    /// <typeparam name="T">The underlying type of the payload value.</typeparam>
    public class Result<T> : Result
    {
        private readonly T? _value;

        /// <summary>
        /// Gets the payload value when the operation succeeds.
        /// Throws an <see cref="InvalidOperationException"/> if accessed on a failed result.
        /// </summary>
        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException($"Cannot access '{nameof(Value)}' on a failed result. Error: {Error}");

        protected Result(T? value, bool isSuccess, string? error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        /// <summary>
        /// Creates a successful operation outcome carrying the specified value.
        /// </summary>
        public static Result<T> Success(T value) => new(value, true, null);

        /// <summary>
        /// Creates a failed operation outcome with a specific error description.
        /// </summary>
        public static new Result<T> Failure(string error) => new(default, false, error);
    }
}
