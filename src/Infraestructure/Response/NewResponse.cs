using Infrastructure;

namespace Infraestructure.Response
{
    public abstract record NewResponse<T>
    {
        public ResponseStatus Status { get; init; } = ResponseStatus.NoDefined;
        public T? Data { get; init; }
        public ISet<string> Messages { get; init; } = new HashSet<string>();

        public bool IsSuccess => Messages.Count == 0;
        public bool IsFailure => !IsSuccess;

        protected NewResponse(ResponseStatus status) => Status = status;
        protected NewResponse(ResponseStatus status, T data)
        {
            Status = status;
            Data = data;
        }
        protected NewResponse(ResponseStatus status, string message)
        {
            Status = status;
            Messages.Add(message);
        }

        protected NewResponse(ResponseStatus status, IEnumerable<string> messages)
        {
            Status = status;
            Messages.UnionWith(messages);
        }
    }
}
