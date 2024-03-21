using Microsoft.AspNetCore.Mvc;

namespace MagicBook.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult NotFound()
        {
            HttpContext.Response.StatusCode = 404;
            return View();
        }
    }
}
