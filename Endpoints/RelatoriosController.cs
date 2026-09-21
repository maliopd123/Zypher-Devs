using Microsoft.AspNetCore.Mvc;
using ProjetoChaves.Data;

namespace ProjetoChaves.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RelatoriosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("relatorios")]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.TotalUsuarios = _context.Usuarios.Count();
            ViewBag.TotalChaves = _context.Chaves.Count();
            ViewBag.TotalNegacoes = _context.Negacoes.Count();
            ViewBag.ChavesDisponiveis = _context.Chaves.Count(c => c.Disponivel);
            return View();
        }
    }
}
