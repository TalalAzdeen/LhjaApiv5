namespace ASG.Api2.Results
{
    public sealed record Error(string title, string message, int Code)
    {
        private static readonly string _recordNotFoundCode = "RecordNotFound";
        private static readonly string _validationErrorCode = "ValidationError";

        public static readonly Error None = new(string.Empty, string.Empty, 0);
        public static Error RecordNotFound(string message) => new Error(_recordNotFoundCode, message, StatusCodes.Status404NotFound);
        public static Error ProblemDetails(string message, int code) =>
            new Error(_recordNotFoundCode, message, code);

        public static Error ValidationError(string message) => new Error(_validationErrorCode, message, StatusCodes.Status400BadRequest);
        public static Error Unauthorized(string message) => new Error("Unauthorized", message, StatusCodes.Status401Unauthorized);
    }
}
