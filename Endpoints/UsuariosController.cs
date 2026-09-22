using Microsoft.AspNetCore.Mvc;
using ProjetoChaves.DAO;
using ProjetoChaves.Models;

namespace ProjetoChaves.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioDAO _usuarioDAO;

        public UsuariosController(UsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        [HttpGet]
        [Route("usuarios")]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Login", "Auth");
            }

            var usuarios = _usuarioDAO.Listar();

            usuarios = usuarios.OrderBy(u => u.Nome).ToList();

            return View(usuarios);
        }
    }
}
