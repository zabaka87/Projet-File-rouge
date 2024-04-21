using Microsoft.AspNetCore.Mvc; // Importe les classes et les interfaces n�cessaires pour cr�er des contr�leurs MVC.
using MagicBook.Models; // Importe les mod�les n�cessaires pour interagir avec les donn�es de l'application.
using System.Diagnostics; // Importe les classes permettant d'acc�der aux informations de diagnostic.
using MySql.Data.MySqlClient; // Importe le fournisseur de donn�es MySQL pour �tablir une connexion � la base de donn�es MySQL.
using System.Configuration; // Importe la classe ConfigurationManager pour acc�der aux informations de configuration.
using Dapper; // Importe Dapper, un micro-ORM qui permet de mapper les r�sultats des requ�tes SQL aux objets .NET.

namespace MagicBook.Controllers // D�finit le namespace MagicBook.Controllers pour le contr�leur HomeController.
{
    public class HomeController : Controller // D�finit la classe HomeController qui h�rite de la classe Controller.
    {
        private readonly ILogger<HomeController> _logger; // D�clare un champ pour enregistrer les journaux.

        private readonly string _connexionString; // D�clare une variable de cha�ne pour la connexion � la base de donn�es.

        // Constructeur de la classe HomeController qui prend ILogger et IConfiguration comme param�tres.
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger; // Initialise le champ du journal.
            _connexionString = configuration.GetConnectionString("MagicBook")!; // Initialise la cha�ne de connexion.
        }

        // Action Index qui renvoie la vue Index avec une liste de livres.
        public IActionResult Index(string message = "")
        {
            string query = "SELECT * FROM Livre ORDER BY @orderBy LIMIT 10;"; // Requ�te SQL pour s�lectionner les 10 premiers livres tri�s par ordre sp�cifi�.
            List<Livre> livres; // Initialise une liste pour stocker les livres.
            using (var connexion = new MySqlConnection(_connexionString))
            {
                livres = connexion.Query<Livre>(query, new { orderBy = "DatePublicationLivre" }).ToList(); // Ex�cute la requ�te SQL et r�cup�re les livres.
                ViewData["livres"] = livres; // Stocke les livres dans ViewData pour les utiliser dans la vue.
            }
            if (message.Length > 0)
            {
                ViewData["ValidateMessage"] = message; // Ajoute un message de validation � ViewData si n�cessaire.
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

        // Action Error qui renvoie la vue Error avec les d�tails de l'erreur.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); // Renvoie la vue Error avec les d�tails de l'erreur.
        }

        // Action Contact qui renvoie la vue Contact.
        public IActionResult Contact()
        {
            return View(); // Renvoie la vue Contact.
        }

    }
}

