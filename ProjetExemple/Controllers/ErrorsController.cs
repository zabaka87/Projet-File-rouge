using Microsoft.AspNetCore.Mvc;

namespace ProjetExemple.Controllers
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
