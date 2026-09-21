using Microsoft.AspNetCore.Mvc;

namespace ProjetoChaves.Controllers
{
    public class ConfiguracoesController : Controller
    {
        [HttpGet]
        [Route("configuracoes")]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
    }
}
