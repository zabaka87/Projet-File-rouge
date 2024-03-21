using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetExemple.Models;
using System.Diagnostics;
using Dapper;

namespace ProjetExemple.Controllers
{
    public class LivreController : Controller
    {
        private readonly string _connexionString;
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
    }
}
