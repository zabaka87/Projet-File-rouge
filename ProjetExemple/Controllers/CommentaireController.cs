using Microsoft.AspNetCore.Mvc;

namespace ProjetExemple.Controllers
{
    public class CommentaireController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
