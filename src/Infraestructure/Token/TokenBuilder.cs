using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infraestructure.Token
{
    public class TokenBuilder
    {
        private int _tokenDurationInHours = 1;
        private string _userId = string.Empty;
        private string _username = string.Empty;
        private string _timeZone = string.Empty;
        private string _jwtIssuer = string.Empty;
        private string _jwtKey = string.Empty;

        public TokenBuilder WithUserId(Guid userId)
        {
            _userId = userId.ToString();
            return this;
        }
        public TokenBuilder WithUsername(string username)
        {
            _username = username;
            return this;
        }
        public TokenBuilder WithJwtIssuer(string jwtIssuer)
        {
            _jwtIssuer = jwtIssuer;
            return this;
        }
        public TokenBuilder WithJwtKey(string jwtKey)
        {
            _jwtKey = jwtKey;
            return this;
        }
        public TokenBuilder WithTokenDurationInHours(int tokenDurationInHours)
        {
            _tokenDurationInHours = tokenDurationInHours;
            return this;
        }
        public TokenBuilder WithTimeZone(decimal timeZone)
        {
            _timeZone = timeZone.ToString();
            return this;
        }

        public string Build()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(CustomClaims.UserId, _userId),
                new Claim(CustomClaims.UserName, _username),
                new Claim(CustomClaims.TimeZone, _timeZone),
            };

            var utcDateTime = DateTime.UtcNow;
            var expiresAt = utcDateTime.AddHours(_tokenDurationInHours);

            var token = new JwtSecurityToken(
                _jwtIssuer,
                _jwtIssuer,
                notBefore: utcDateTime,
                expires: expiresAt,
                claims: claims,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
