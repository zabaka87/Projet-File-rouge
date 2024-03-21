using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using ProjetExemple.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Diagnostics;

namespace ProjetExemple.Controllers
{
    public class UtilisateurController : Controller
    {
        private string _connexionString;

        public UtilisateurController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MagicBook")!;
        }

        public IActionResult Index(string message = "")
        {
            string query = "SELECT * FROM Utilisateur";
            List<Utilisateur> utilisateurs;
            using (var connexion = new MySqlConnection(_connexionString))
            {
                 utilisateurs = connexion.Query<Utilisateur>(query).ToList();
            }
            if (message.Length > 0)
            {
                ViewData["ValidateMessage"] = message;
            }
            return View(utilisateurs);
        }


        [HttpGet] // méthode retournant le formulaire permettant de créer une personnage
        public IActionResult Ajouter()
        {
            return View(GenerateAjoutPersonnageViewModel());
        }

        [HttpPost] // méthode permettant de gérer la soumission du formulaire de création de personnage
        public IActionResult Ajouter(Utilisateur utilisateur)
        {
            Console.WriteLine(Request.Form.Files.Count);
           
            string InsertQuery = "INSERT INTO Utilisateur(IdUtilisateur,Administrateur,NomUtilisateur,PrenomUtilisateur,PseudoUtilisateur,EmailUtilisateur,Adresse1,Adresse2,CodePostal,Ville,NbJetons,DateInscription) VALUES (@IdUtilisateur,@Administrateur,@NomUtilisateur,@PrenomUtilisateur,@PseudoUtilisateur,@EmailUtilisateur,@Adresse1,@Adresse2,@CodePostal,@Ville,@NbJetons,@DateInscription)";
           
            string? filePath = null;

            if (Request.Form.Files.Count == 1 && Request.Form.Files[0].Length > 0)
            {
                filePath = Path.Combine("/utilisateur/",
                    Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(Request.Form.Files[0].FileName)).ToString();

                using (var stream = System.IO.File.Create("wwwroot" + filePath))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                
            }

            using (var connexion = new MySqlConnection(_connexionString))
            {
                try
                {
                    int RowsAdded = connexion.Execute(InsertQuery, utilisateur);
                    if (RowsAdded <= 0)
                    {
                        ViewData["ValidateMessage"] = "L'inscription n'a pas fonctionné";
                    }
                    else
                    {
                        ViewData["ValidateMessage"] = "L'inscription a fonctionné";
                    }
                }
                catch (MySqlException e)
                {
                    ViewData["ValidateMessage"] = "L'inscription n'a pas fonctionné : " + e.Message;
                }


            }
            return View(GenerateAjoutPersonnageViewModel());


        }
    }
}
