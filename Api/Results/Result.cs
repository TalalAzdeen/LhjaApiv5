using Microsoft.AspNetCore.Mvc;

namespace ASG.Api2.Results
{
    public class Result
    {
        private readonly object _value;

        public bool IsSuccess { get; }
        public bool IsFailuer => !IsSuccess;

        public object Data
        {
            get
            {
                //if (!IsSuccess) throw new InvalidOperationException("there is no value for failure");
                return _value;
            }
            private init => _value = value;
        }

        public Error? Error { get; private set; }

        public Result(object value)
        {
            Data = value;
            IsSuccess = true;
            //Error = Error.None;
        }

        public Result(bool IsSuccess = true) { this.IsSuccess = IsSuccess; }

        public Result(Error error)
        {
            if (error == Error.None)
                throw new ArgumentException("Invalid error");
            IsSuccess = false;
            Error = error;
        }

        public static Result Ok()
        {
            return new Result();
        }

        public static Result Ok(object value)
        {
            var r = new Result(value);
            return r;
        }
        public static Result Failure(Error error) => new(error);
        public static ProblemDetails Problem(Exception ex)
        {
            return new ProblemDetails
            {
                Type = ex.GetType().FullName,
                Title = ex.Message,
                Detail = ex.InnerException?.Message,

                //Status=ex.cod
            };
        }
        public static implicit operator Result(Error error)
        {
            return Failure(error);
        }
        public static ProblemDetails Problem(string title, string details, string? type = null, int? status = StatusCodes.Status500InternalServerError)
        {
            return new ProblemDetails
            {
                Title = title,
                Detail = details,
                Type = type,
                Status = status
            };
        }

        public static ProblemDetails NotFound(string details, string? type = null, string title = "Not Found", int status = StatusCodes.Status404NotFound)
        {
            return new ProblemDetails
            {
                Title = title,
                Detail = details,
                Type = type,
                Status = status
            };
        }

    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public bool IsSuccess { get; }

        public TValue? Value
        {
            get
            {
                //if (!IsSuccess) throw new InvalidOperationException("there is no value for failure");
                return _value;
            }
            private init => _value = value;
        }

        public Error? Error { get; private set; }


        public Result(TValue value)
        {
            Value = value;
            IsSuccess = true;
            //Error = Error.None;
        }

        public Result(Error error)
        {
            if (error == Error.None)
                throw new ArgumentException("Invalid error");
            IsSuccess = false;
            Error = error;
        }
        public static Result<TValue> Ok<TValue>(TValue value)
        {
            var result = new Result<TValue>(value);
            return result;
        }
        public static Result<TValue> Success(TValue value)
        {

            var r = new Result<TValue>(value);
            return r;
        }
        public static Result Failure(Error error) => new(error);

    }
}
