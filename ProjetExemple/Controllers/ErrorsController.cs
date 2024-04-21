using Microsoft.AspNetCore.Mvc; // Importe les classes et les interfaces nécessaires pour créer des contrôleurs MVC.

namespace MagicBook.Controllers // Définit le namespace MagicBook.Controllers pour le contrôleur ErrorsController.
{
    public class ErrorsController : Controller // Définit la classe ErrorsController qui hérite de la classe Controller.
    {
        // Action NotFound qui renvoie la vue NotFound en cas d'erreur 404.
        public IActionResult NotFound()
        {
            HttpContext.Response.StatusCode = 404; // Définit le code de statut de la réponse HTTP comme 404 (Page non trouvée).
            return View(); // Renvoie la vue NotFound pour afficher un message d'erreur correspondant à l'erreur 404.
        }
    }
}
