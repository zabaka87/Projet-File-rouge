using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetExemple.Models;

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
                List<PersonnageViewModel> personnages = connexion.Query<PersonnageViewModel>(query).ToList();
                ViewData["personnages"]=personnages;
            }
            return View();
        }

        public IActionResult Detail(int id)
        {
            string query = "SELECTE * FROM RACE INNER JOIN PERSONNAGE ON PERSONNAGE.codeRace = RACE.codeRace WHERE codeRace=@id;";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                List<Race>races = connexion.Query<Race>(query, new {id = id}).ToList();
                ViewData["RACES"] = "detail race";

                var viewModel = new PersonnageViewModel() { races };
            return View(viewModel);
        }
    }
}

