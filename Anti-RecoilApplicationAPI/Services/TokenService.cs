using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Anti_RecoilApplicationAPI.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private static readonly Dictionary<string, string> _tokenStore = new Dictionary<string, string>();

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void AddToken(string userId, string token)
        {
            _tokenStore[userId] = token;
        }

        public void Remove(string userId)
        {
            _tokenStore.Remove(userId);
        }

        public bool IsTokenValid(string userId, string token)
        {
            return _tokenStore.TryGetValue(userId, out var storedToken) && storedToken == token;
        }

        public string GenerateToken(string username, string role)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var secretKey = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("Jwt:SecretKey");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new ArgumentNullException("Jwt:Issuer or Jwt:Audience");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = creds,
                Issuer = issuer,
                Audience = audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
