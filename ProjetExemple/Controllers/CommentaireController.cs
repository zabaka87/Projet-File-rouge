using Microsoft.AspNetCore.Mvc;
using MagicBook.Models;

namespace MagicBook.Controllers
{
    public class CommentaireController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
