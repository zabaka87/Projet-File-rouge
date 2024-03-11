using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetExemple.Models;

namespace ProjetExemple.Controllers
{
    public class RaceController : Controller
    {
        private readonly string _connexionString;
        public RaceController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MargotEtLesElfes")!;
        }
        public IActionResult Index(string id)
        {
            string query = "SELECT * FROM RACE WHERE codeRace=@id";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                List<Race> Races = connexion.Query<Race>(query, new {idcodeRace = id}).ToList();
                ViewData["Races"] = Races;
            }
            return View("index");
        }
    }
}
