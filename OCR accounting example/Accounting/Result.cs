namespace Accounting
{
    public sealed class Result<T>
    {
        public bool IsOk { get; }
        public T? Value { get; }
        public string? Error { get; }


        private Result(bool isOk, T? value, string? error)
        {
            IsOk = isOk;
            Value = value;
            Error = error;
        }

        public static Result<T> Ok(T value)
        {
            return new(true, value, null);
        }

        public static Result<T> Failure(string error)
        {
            return new(false, default, error);
        }
    }
}
