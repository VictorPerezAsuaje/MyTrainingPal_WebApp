namespace MyTrainingPal.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && error != string.Empty)
                throw new InvalidOperationException();
            if (!isSuccess && error == string.Empty)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Fail(string error) => new Result(false, error);
        public static Result<T> Fail<T>(string error) => 
            new Result<T>(default(T), false, error);

        public static Result Ok() 
            => new Result(true, string.Empty);
        public static Result<T> Ok<T>(T value) 
            => new Result<T>(value, true, string.Empty);
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess) throw new InvalidOperationException();
                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string error) : base(isSuccess, error)
        {
            _value = value;
        }
    }
}
