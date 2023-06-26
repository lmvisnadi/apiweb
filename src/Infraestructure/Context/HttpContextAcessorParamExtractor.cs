using Infraestructure.Token;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infraestructure.Context
{
    public class HttpContextAcessorParamExtractor
    {
        private IHttpContextAccessor ContextAccessor;

        public HttpContextAcessorParamExtractor(IHttpContextAccessor contextAccessor)
        {
            ContextAccessor = contextAccessor;
        }

        public string GetToken()
        {
            var bearerToken = GetHeaderValue("Authorization");
            var tokenPart = 1;
            var whitespace = ' ';

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return string.Empty;
            }
            return bearerToken.Split(whitespace)[tokenPart];
        }

        public Guid GetUserGuid() => Guid.TryParse(GetClaimValue(CustomClaims.UserId), out Guid userId) ? userId : Guid.Empty;

        public string GetUserName() => GetClaimValue(CustomClaims.UserName);

        public decimal GetTimeZone() => Decimal.TryParse(GetClaimValue(CustomClaims.TimeZone), out decimal timezone) ? timezone : 0;


        //public List<Guid> GetParticipanteAcessos() => JsonConvert.DeserializeObject<List<Guid>>(GetClaimValue(CustomClaims.ParticipanteAcessoIds));

        private string GetHeaderValue(string search)
            => Headers?.FirstOrDefault(header => search.Equals(header.Key)).Value.ToString() ?? string.Empty;

        private string GetClaimValue(string search)
            => Claims?.Where(claim => claim.Type == search).FirstOrDefault()?.Value ?? string.Empty;

        private IHeaderDictionary? Headers => ContextAccessor.HttpContext?.Request.Headers;

        private IEnumerable<Claim>? Claims => ContextAccessor.HttpContext?.User.Claims;

    }
}
