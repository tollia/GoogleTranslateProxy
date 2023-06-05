using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoogleTranslateProxy.Models
{
    public class AccessToken
    {
        public string SecretKey { get; set; }
        private string Issuer { get; }
        private string Audience { get; }
        private int ExpirationMinutes { get; }
        private string[] Roles { get; }

        public AccessToken(
            string secretKey,
            string issuer,
            string audience,
            int expirationMinutes,
            string[] roles)
        {
            SecretKey = secretKey;
            Issuer = issuer;
            Audience = audience;
            ExpirationMinutes = expirationMinutes;
            Roles = roles;
        }

        public string Get()
        {
            return GenerateJwtToken(SecretKey, Issuer, Audience, ExpirationMinutes, Roles);
        }


        private static string GenerateJwtToken(string secretKey, string issuer, string audience, int expirationMinutes, string[] roles)
        {
            // Create claims for the token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "username")
            };

            // Add roles to claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Create signing credentials using the security key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = creds
            };

            // Create token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Generate token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the generated token
            return tokenHandler.WriteToken(token);
        }
    }
}
