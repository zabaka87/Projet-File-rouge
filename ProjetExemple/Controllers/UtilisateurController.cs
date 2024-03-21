using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MagicBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Diagnostics;

namespace MagicBook.Controllers
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
       
        public IActionResult Ajouter()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Inscription()
        {
            return View();
        }
    }
}
