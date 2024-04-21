using Microsoft.AspNetCore.Mvc; // Importe les classes et les interfaces nécessaires pour créer des contrôleurs MVC.
using Microsoft.Extensions.Configuration; // Importe les fonctionnalités de configuration pour accéder aux paramètres de l'application.
using Dapper; // Importe Dapper, un micro-ORM qui permet de mapper les résultats des requêtes SQL aux objets .NET.
using MySql.Data.MySqlClient; // Importe le fournisseur de données MySQL pour établir une connexion à la base de données MySQL.
using System; // Importe les types et les classes de base du .NET Framework.
using System.Collections.Generic; // Importe les types et les classes pour travailler avec des collections génériques.
using MagicBook.Models; // Importe les modèles nécessaires pour interagir avec les données de l'application.
using Microsoft.AspNetCore.Mvc.Rendering; // Importe les classes pour générer des éléments de formulaire HTML.
using System.IO; // Importe les fonctionnalités de gestion des fichiers et des dossiers.
using static System.Net.WebRequestMethods; // Importe les méthodes de requ


namespace MagicBook.Controllers
{
    // Définition du namespace MagicBook.Controllers et de la classe UtilisateurController
    public class UtilisateurController : Controller
    {
        private string _connexionString; // Déclaration d'une variable de chaîne de connexion privée

        // Constructeur de la classe UtilisateurController qui prend IConfiguration comme paramètre
        public UtilisateurController(IConfiguration configuration)
        {
            // Initialisation de la chaîne de connexion en utilisant la méthode GetConnectionString de l'objet IConfiguration
            _connexionString = configuration.GetConnectionString("MagicBook")!;
        }

        // Action Index qui retourne une vue avec une liste d'utilisateurs
        public IActionResult Index(string message = "")
        {
            // Définition de la requête SQL pour sélectionner tous les utilisateurs
            string query = "SELECT * FROM Utilisateur";

            List<Utilisateur> utilisateurs; // Déclaration d'une liste d'utilisateurs

            // Utilisation d'une instruction using pour garantir la fermeture automatique de la connexion
            using (var connexion = new MySqlConnection(_connexionString))
            {
                // Exécution de la requête SQL et récupération des utilisateurs dans la liste
                utilisateurs = connexion.Query<Utilisateur>(query).ToList();
            }

            // Vérification de la longueur du message et ajout du message à la ViewData s'il est non vide
            if (message.Length > 0)
            {
                ViewData["ValidateMessage"] = message;
            }

            // Retourne la vue Index avec la liste des utilisateurs
            return View(utilisateurs);
        }
       
    }
}
    




