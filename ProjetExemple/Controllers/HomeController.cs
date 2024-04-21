using Microsoft.AspNetCore.Mvc; // Importe les classes et les interfaces nécessaires pour créer des contrôleurs MVC.
using MagicBook.Models; // Importe les modèles nécessaires pour interagir avec les données de l'application.
using System.Diagnostics; // Importe les classes permettant d'accéder aux informations de diagnostic.
using MySql.Data.MySqlClient; // Importe le fournisseur de données MySQL pour établir une connexion à la base de données MySQL.
using System.Configuration; // Importe la classe ConfigurationManager pour accéder aux informations de configuration.
using Dapper; // Importe Dapper, un micro-ORM qui permet de mapper les résultats des requêtes SQL aux objets .NET.

namespace MagicBook.Controllers // Définit le namespace MagicBook.Controllers pour le contrôleur HomeController.
{
    public class HomeController : Controller // Définit la classe HomeController qui hérite de la classe Controller.
    {
        private readonly ILogger<HomeController> _logger; // Déclare un champ pour enregistrer les journaux.

        private readonly string _connexionString; // Déclare une variable de chaîne pour la connexion à la base de données.

        // Constructeur de la classe HomeController qui prend ILogger et IConfiguration comme paramètres.
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger; // Initialise le champ du journal.
            _connexionString = configuration.GetConnectionString("MagicBook")!; // Initialise la chaîne de connexion.
        }

        // Action Index qui renvoie la vue Index avec une liste de livres.
        public IActionResult Index(string message = "")
        {
            string query = "SELECT * FROM Livre ORDER BY @orderBy LIMIT 10;"; // Requête SQL pour sélectionner les 10 premiers livres triés par ordre spécifié.
            List<Livre> livres; // Initialise une liste pour stocker les livres.
            using (var connexion = new MySqlConnection(_connexionString))
            {
                livres = connexion.Query<Livre>(query, new { orderBy = "DatePublicationLivre" }).ToList(); // Exécute la requête SQL et récupère les livres.
                ViewData["livres"] = livres; // Stocke les livres dans ViewData pour les utiliser dans la vue.
            }
            if (message.Length > 0)
            {
                ViewData["ValidateMessage"] = message; // Ajoute un message de validation à ViewData si nécessaire.
            }

            return View(); // Renvoie la vue Index pour afficher les livres.
        }

        // Action Privacy qui renvoie la vue Privacy.
        public IActionResult Privacy()
        {
            return View(); // Renvoie la vue Privacy.
        }

        // Action APropos qui renvoie la vue APropos.
        public IActionResult APropos()
        {
            return View(); // Renvoie la vue APropos.
        }

        // Action Error qui renvoie la vue Error avec les détails de l'erreur.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); // Renvoie la vue Error avec les détails de l'erreur.
        }

        // Action Contact qui renvoie la vue Contact.
        public IActionResult Contact()
        {
            return View(); // Renvoie la vue Contact.
        }

    }
}

