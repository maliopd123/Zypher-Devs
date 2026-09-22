using Microsoft.AspNetCore.Mvc;

namespace ProjetoChaves.Controllers
{
    public class SuporteController : Controller
    {
        [HttpGet]
        [Route("ajuda")]
        public IActionResult Ajuda()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpGet]
        [Route("documentacao")]
        public IActionResult Documentacao()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
    }
}
