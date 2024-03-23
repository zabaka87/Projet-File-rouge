using Dapper;
using MagicBook.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace MagicBook.Controllers
{
    public class CategorieController : Controller
    {
        private readonly string _connexionString;
        public CategorieController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MagicBook")!;
        }
        public IActionResult GetCategorie(string message = "")
        {
            List<Categorie> categories = new List<Categorie>();
            string query = "SELECT * FROM Categorie";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                 categories = connexion.Query<Categorie>(query).ToList();
                ViewData["Categories"] = categories;
            }
            if (message.Length > 0)
            {
                ViewData["ValidateMessage"] = message;
            }
            return (IActionResult)categories;
        }
    }
}
