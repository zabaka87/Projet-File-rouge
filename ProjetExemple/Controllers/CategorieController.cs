using Dapper; // Importe Dapper, un micro-ORM qui permet de mapper les résultats des requêtes SQL aux objets .NET.
using MagicBook.Models; // Importe les modèles nécessaires pour interagir avec les données de l'application.
using Microsoft.AspNetCore.Mvc; // Importe les classes et les interfaces nécessaires pour créer des contrôleurs MVC.
using MySql.Data.MySqlClient; // Importe le fournisseur de données MySQL pour établir une connexion à la base de données MySQL.

namespace MagicBook.Controllers // Définit le namespace MagicBook.Controllers pour le contrôleur CategorieController.
{
    public class CategorieController : Controller // Définit la classe CategorieController qui hérite de la classe Controller.
    {
        private readonly string _connexionString; // Déclare une variable de chaîne pour la connexion à la base de données.

        // Constructeur de la classe CategorieController qui prend IConfiguration comme paramètre pour obtenir la chaîne de connexion.
        public CategorieController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MagicBook")!; // Initialise la chaîne de connexion.
        }

        // Action GetCategorie qui récupère les catégories depuis la base de données et les affiche.
        public IActionResult GetCategorie(string message = "")
        {
            List<genres> categories = new List<genres>(); // Initialise une liste pour stocker les catégories.
            string query = "SELECT * FROM Categorie"; // Requête SQL pour sélectionner toutes les catégories.
            using (var connexion = new MySqlConnection(_connexionString))
            {
                categories = connexion.Query<genres>(query).ToList(); // Exécute la requête SQL et récupère les catégories.
                ViewData["Categories"] = categories; // Stocke les catégories dans ViewData pour les utiliser dans la vue.
            }
            if (message.Length > 0)
            {
                ViewData["ValidateMessage"] = message; // Ajoute un message de validation à ViewData si nécessaire.
            }
            return (IActionResult)categories; // Retourne les catégories à afficher dans la vue.
        }
    }
}
