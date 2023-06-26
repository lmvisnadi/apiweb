using Infrastructure;

namespace Infraestructure.Response
{
    public record UnprocessableResponse<T> : NewResponse<T>
    {
        public UnprocessableResponse(string message)
            : base(ResponseStatus.Unprocessable, message) { }
        public UnprocessableResponse(IEnumerable<string> messages)
            : base(ResponseStatus.Unprocessable, messages) { }
    }
}
