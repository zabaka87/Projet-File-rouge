using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MagicBook.Models;
using System.Diagnostics;
using Dapper;
using MagicBook.Models;

namespace MagicBook.Controllers
{
    public class LivreController : Controller
    {
        private readonly string _connexionString;
        //J'ai appeler le contrroller categorie pour l'afficher dans livre
        private readonly CategorieController _categorieController;
        public LivreController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MagicBook")!;
        }
        public IActionResult Index(string message = "")
        {
            string query = "SELECT * FROM Livre";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                List<Livre> Livres = connexion.Query<Livre>(query).ToList();
                ViewData["Livres"] = Livres;
            }
            if (message.Length > 0)
            {
                ViewData["ValidateMessage"] = message;
            }
            return View("index");
        }

        [HttpGet] // méthode retournant le formulaire permettant de créer une race
        public IActionResult Ajouter()
        {
            return View();
        }

        [HttpPost] // méthode permettant de gérer la soumission du formulaire de création de livre
        public IActionResult Ajouter(Livre livre)
        {
            // validation du model par rapport aux annotations
            // attention : ne vérifie pas si la clé primaire est déjà utilisée ou pas
            // ne vérifie pas non plus si les clés étrangères existent
            if (!ModelState.IsValid)
            {
                ViewData["ValidateMessage"] = "Les données fournis ne correspondent pas à ce qui est demandé.";
                return View();
            }

            // on fait une requete select sur la base de données pour savoir si la clé primaire existe déja.
            string ClePremaireQuery = "SELECT COUNT(*) FROM Livre WHERE ISBN=@ISBN";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                int res = connexion.ExecuteScalar<int>(ClePremaireQuery, new { ISBN = livre.ISBN });
                if (res > 0)
                {
                    ViewData["ValidateMessage"] = "Le code fourni est déjà utilisé. Veuillez en essayer un autre.";
                    return View();
                }
            }
            string InsertQuery = "INSERT INTO Livre(ISBN,TitreLivre,ResumeLivre,DatePublicationLivre,CouvertureLivre,) VALUES (@ISBN,@TitreLivre,@ResumeLivre,@DatePublicationLivre,@CouvertureLivre)";

            using (var connexion = new MySqlConnection(_connexionString))
            {
                try
                {
                    int RowsAdded = connexion.Execute(InsertQuery, livre);
                    if (RowsAdded <= 0)
                    {
                        ViewData["ValidateMessage"] = "La création du livre n'a pas fonctionné";
                    }
                    else
                    {
                        ViewData["ValidateMessage"] = "La création du livre a fonctionné";
                    }
                }
                catch (MySqlException)
                {
                    ViewData["ValidateMessage"] = "La création du livre n'a pas fonctionné : ";
                }
            }
            return View();
        }
    }
}
