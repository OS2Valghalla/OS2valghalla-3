namespace Valghalla.Application.Abstractions.Messaging
{
    public class Response
    {
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public Confirmation? Confirmation { get; set; }
        public IReadOnlyDictionary<string, string[]>? Errors { get; set; }

        public Response()
        {
            IsSuccess = true;
        }
        public Response(string msg, IReadOnlyDictionary<string, string[]>? errors)
        {
            Message = msg;
            IsSuccess = false;
            Errors = errors;
        }

        public Response(string msg)
        {
            Message = msg;
            IsSuccess = false;
        }

        public bool IsNotFound() => Message == "shared.error.common.not_found";

        public static Response<T> Ok<T>(T? data = default) => new(data);
        public static Response Ok() => new();
        public static Response FailWithError(string msg, IReadOnlyDictionary<string, string[]>? errors) => new(msg, errors);
        public static Response Fail(string msg) => new(msg);
        public static Response<T> Fail<T>(string msg, T? data = default) => new(msg, data);
        public static Response FailWithItemNotFoundError() => Fail("shared.error.common.not_found");
    }

    public sealed class Response<T> : Response
    {
        // parameterless constructor to allow validation pipeline manually create it
        public Response() { }

        public Response(string msg, IReadOnlyDictionary<string, string[]> errors) : base(msg, errors)
        {
        }

        public Response(T? data)
        {
            Data = data;
        }

        public Response(string msg, T? data)
        {
            Data = data;
            Message = msg;
            IsSuccess = false;
        }

        public T? Data { get; set; }
    }
}
