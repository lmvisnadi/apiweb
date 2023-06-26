using Infrastructure;

namespace Infraestructure.Response
{
    public record UnauthorizedResponse<T> : NewResponse<T>
    {
        public UnauthorizedResponse(string message)
            : base(ResponseStatus.Unauthorized, message) { }
    }
}
