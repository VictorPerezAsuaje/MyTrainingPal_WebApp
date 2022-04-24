namespace MyTrainingPal.Domain.Common
{
    public enum ResultType
    {
        NullParameter,
        EmptyString,
        ObjectNotFound,
        EmptyList,
        LackRequiredElements,
        IntegerValueNotValid,
        GenericProcessError,
        Ok
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public Tuple<ResultType, string> Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, Tuple<ResultType, string> error)
        {
            if (isSuccess && error.Item2 != string.Empty)
                throw new InvalidOperationException();
            if (!isSuccess && error.Item2 == string.Empty)
                throw new InvalidOperationException();
            if (error.Item1 == ResultType.Ok && error.Item2 != string.Empty)
                throw new InvalidOperationException();
            if (error.Item1 != ResultType.Ok && error.Item2 == string.Empty)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Fail(Tuple<ResultType, string> error) => new Result(false, error);
        public static Result<T> Fail<T>(Tuple<ResultType, string> error) => 
            new Result<T>(default(T), false, error);

        public static Result Ok() 
            => new Result(true, new Tuple<ResultType, string>(ResultType.Ok, string.Empty));
        public static Result<T> Ok<T>(T value) 
            => new Result<T>(value, true, new Tuple<ResultType, string>(ResultType.Ok, string.Empty));
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

        protected internal Result(T value, bool isSuccess, Tuple<ResultType, string> error) : base(isSuccess, error)
        {
            _value = value;
        }
    }
}
