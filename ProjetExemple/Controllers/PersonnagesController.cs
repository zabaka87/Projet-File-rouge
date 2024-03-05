using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetExemple.Models;
using Dapper;
namespace ProjetExemple.Controllers
{
    public class PersonnagesController : Controller
    {
        private readonly string _connexionString;
        public PersonnagesController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MargotEtLesElfes")!;
        }
        public IActionResult Index()
        {
            string query = "SELECT * FROM PERSONNAGE";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                List<Personnage> personnages = connexion.Query<Personnage>(query).ToList();
                ViewData["personnages"] = personnages;
            }
            return View();
        }
    }
}
