using Microsoft.AspNetCore.Mvc;
using ProjetoChaves.DAO;
using ProjetoChaves.Models;
using System.Security.Cryptography;
using System.Text;

namespace ProjetoChaves.Controllers
{
    public class AuthController : Controller
    {
        private readonly UsuarioDAO _usuarioDAO;

        public AuthController(UsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string senha)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                ViewBag.Erro = "Preencha todos os campos";
                return View();
            }

            var usuario = _usuarioDAO.BuscarPorEmail(email.Trim());
            if (usuario == null || string.IsNullOrEmpty(usuario.Senha) || !VerificarSenha(senha, usuario.Senha))
            {
                ViewBag.Erro = "E-mail ou senha invalida";
                return View();
            }

            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Cadastro(string nome, string email, string senha, string confirmarSenha)
        {
            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(senha) ||
                string.IsNullOrEmpty(confirmarSenha))
            {
                ViewBag.Erro = "Preencha todos os campos";
                return View();
            }

            if (senha != confirmarSenha)
            {
                ViewBag.Erro = "As senhas nao conferem";
                return View();
            }

            if (senha.Length < 6)
            {
                ViewBag.Erro = "A senha deve ter no minimo 6 caracteres";
                return View();
            }

            if (_usuarioDAO.BuscarPorEmail(email.Trim()) != null)
            {
                ViewBag.Erro = "E-mail ja cadastrado";
                return View();
            }

            var usuario = new Usuario
            {
                Nome = nome,
                Email = email,
                Senha = HashSenha(senha),
                DataCadastro = DateTime.Now
            };

            _usuarioDAO.Inserir(usuario);

            TempData["Sucesso"] = "Cadastro realizado com sucesso. Entre com seu e-mail e senha.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("forgot-password")]
        public IActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Erro = "Informe seu email";
                return View();
            }

            ViewBag.Sucesso = true;
            return View();
        }

        [HttpGet]
        [Route("verify-code")]
        public IActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        [Route("verify-code")]
        public IActionResult VerifyCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                ViewBag.Erro = "Por favor, insira o codigo de verificacao";
                return View();
            }

            if (code != "123456")
            {
                ViewBag.Erro = "Codigo de verificacao invalido";
                return View();
            }

            HttpContext.Session.SetString("UsuarioId", "1");
            HttpContext.Session.SetString("UsuarioNome", "Usuario Teste");
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private static string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerificarSenha(string senha, string hash)
        {
            return senha == hash || HashSenha(senha) == hash;
        }
    }
}
