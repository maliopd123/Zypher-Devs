using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginSystem.Models;
using Microsoft.IdentityModel.Tokens;

namespace LoginSystem.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GerarToken(User user)
        {
            var chave = _config["Jwt:Key"]!;
            var emissor = _config["Jwt:Issuer"];
            var audiencia = _config["Jwt:Audience"];
            var horasExpiracao = double.Parse(_config["Jwt:ExpiresInHours"] ?? "8");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("nome", user.Nome),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var credenciais = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: emissor,
                audience: audiencia,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(horasExpiracao),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
