using Microsoft.AspNetCore.Mvc;
using ProjetoChaves.Data;

namespace ProjetoChaves.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool VerificarAutenticacao()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!VerificarAutenticacao())
            {
                return RedirectToAction("Login", "Auth");
            }

            var hasChaves = _context.Chaves.Any();
            var totalChaves = hasChaves ? _context.Chaves.Count() : 32;
            var chavasDisponiveis = hasChaves ? _context.Chaves.Count(c => c.Disponivel) : 24;
            var chavasIndisponiveis = hasChaves ? totalChaves - chavasDisponiveis : 8;
            var totalRegistros = _context.Usuarios.Any() ? _context.Usuarios.Count() : 156;
            var negacoes = _context.Negacoes.OrderByDescending(n => n.DataHora).Take(3).ToList();

            ViewBag.TotalChaves = totalChaves;
            ViewBag.ChavasDisponiveis = chavasDisponiveis;
            ViewBag.ChavasIndisponiveis = chavasIndisponiveis;
            ViewBag.TotalRegistros = totalRegistros;
            ViewBag.Negacoes = negacoes;

            return View();
        }
    }
}
