namespace KeL.Infrastructure.Security.Context
{
    public class RequestContext
    {
        public string Token { get; }
        public Guid UserId { get; }
        public Guid RequestId { get; }
        public string UserName { get; }
        public decimal TimeZone { get; }

        public RequestContext(
            string token,
            Guid userId,
            string userName,
            Guid requestId,
            decimal timeZone)
        {
            Token = token;
            UserId = userId;
            UserName = userName;
            TimeZone = timeZone;
            RequestId = requestId;
        }
    }
}