using Infrastructure;

namespace Infraestructure.Response
{
    public record NoContentResponse<T> : NewResponse<T>
    {
        public NoContentResponse()
            : base(ResponseStatus.NoContent) { }
    }
}
