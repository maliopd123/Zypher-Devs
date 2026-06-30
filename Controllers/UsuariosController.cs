using Microsoft.AspNetCore.Mvc;
using ProjetoChaves.Data;

namespace ProjetoChaves.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("usuarios")]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Login", "Auth");
            }

            var usuarios = _context.Usuarios.OrderBy(u => u.Nome).ToList();
            return View(usuarios);
        }
    }
}
