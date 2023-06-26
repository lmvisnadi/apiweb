namespace Infraestructure.Controller
{
    public struct ApiResponse
    {
        public ApiResponse(string? title, int status, ISet<string> errors)
        {
            Title = title ?? string.Empty;
            Status = status;
            Errors = errors ?? new HashSet<string>();
        }

        public string? Title { get; }
        public int Status { get; }
        public ISet<string> Errors { get; }
    }
}
