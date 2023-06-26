using Infrastructure;

namespace Infraestructure.Response
{
    public record NotFoundResponse<T> : NewResponse<T>
    {
        public NotFoundResponse(string message)
            : base(ResponseStatus.NotFound, message) { }
    }
}
