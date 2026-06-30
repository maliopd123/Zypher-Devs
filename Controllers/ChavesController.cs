using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoChaves.Data;
using ProjetoChaves.Models;

namespace ProjetoChaves.Controllers
{
    public class ChavesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChavesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool VerificarAutenticacao()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId"));
        }

        [HttpGet]
        [Route("chaves")]
        public IActionResult Index()
        {
            if (!VerificarAutenticacao())
            {
                return RedirectToAction("Login", "Auth");
            }

            var chaves = _context.Chaves
                .Include(c => c.UsuarioResponsavel)
                .OrderBy(c => c.Codigo)
                .ToList();

            return View(chaves);
        }

        [HttpGet]
        [Route("chaves-disponiveis")]
        public IActionResult Disponiveis()
        {
            if (!VerificarAutenticacao())
            {
                return RedirectToAction("Login", "Auth");
            }

            var chaves = _context.Chaves.Where(c => c.Disponivel).ToList();
            return View(chaves);
        }

        [HttpGet]
        [Route("disponibilidade")]
        public IActionResult Disponibilidade()
        {
            if (!VerificarAutenticacao())
            {
                return RedirectToAction("Login", "Auth");
            }

            var chaves = _context.Chaves
                .Include(c => c.UsuarioResponsavel)
                .OrderBy(c => c.Codigo)
                .ToList();

            return View(chaves);
        }

        [HttpGet]
        [Route("negacoes")]
        public IActionResult Negacoes()
        {
            if (!VerificarAutenticacao())
            {
                return RedirectToAction("Login", "Auth");
            }

            var negacoes = _context.Negacoes
                .Include(n => n.Usuario)
                .Include(n => n.Chave)
                .OrderByDescending(n => n.DataHora)
                .ToList();

            return View(negacoes);
        }

        [HttpPost]
        [Route("chaves/criar")]
        public IActionResult Criar(string codigo, string descricao, string local, string categoria)
        {
            if (!VerificarAutenticacao())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(descricao))
            {
                TempData["Erro"] = "Codigo e descricao sao obrigatorios";
                return RedirectToAction("Index");
            }

            var chave = new Chave
            {
                Codigo = codigo,
                Descricao = descricao,
                Local = local,
                Categoria = categoria,
                DataCadastro = DateTime.Now,
                Disponivel = true
            };

            _context.Chaves.Add(chave);
            _context.SaveChanges();

            TempData["Sucesso"] = "Chave criada com sucesso";
            return RedirectToAction("Index");
        }
    }
}
