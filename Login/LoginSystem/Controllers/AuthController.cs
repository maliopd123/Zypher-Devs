using LoginSystem.Data;
using LoginSystem.DTOs;
using LoginSystem.Models;
using LoginSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<AuthResponseDto>> Registrar(RegisterDto dto)
        {
            var emailJaExiste = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailJaExiste)
                return Conflict(new { mensagem = "Este e-mail já está cadastrado." });

            var user = new User
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _tokenService.GerarToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Nome = user.Nome,
                Email = user.Email
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            // Mesma mensagem para usuário inexistente ou senha errada (evita enumeração de e-mails)
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, user.SenhaHash))
                return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });

            var token = _tokenService.GerarToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Nome = user.Nome,
                Email = user.Email
            });
        }

        // Exemplo de rota protegida: só acessível com token JWT válido
        [Authorize]
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            var email = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email)?.Value;
            var nome = User.FindFirst("nome")?.Value;

            return Ok(new { mensagem = "Acesso autorizado.", nome, email });
        }
    }
}
