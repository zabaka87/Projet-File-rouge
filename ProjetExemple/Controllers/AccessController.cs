using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MagicBook.Models;
using System.Security.Claims;
using Dapper;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Authorization;

namespace MagicBook.Controllers
{
    public class AccessController : Controller
    {
        private readonly string _connexionString;
        public AccessController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MagicBook")!;
        }
        public IActionResult Login()
        {
            ClaimsPrincipal User = HttpContext.User;
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Utilisateur");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Utilisateur utilisateur)
        {
            string query = "SELECT EmailUtilisateur, Password FROM Utilisateur WHERE EmailUtilisateur = @EmailUtilisateur";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                List<Utilisateur> utilisateurFromDB = connexion.Query<Utilisateur>(query, new { EmailUtilisateur = utilisateur.EmailUtilisateur }).ToList();

                if (utilisateurFromDB.Count > 0 && BC.Verify(utilisateur.Password, utilisateurFromDB[0].Password.ToString()))
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, utilisateur.EmailUtilisateur),
                        new Claim(ClaimTypes.Name, utilisateurFromDB[0].NomUtilisateur),
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = utilisateur.KeepLoggedIn,
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                    if (Request.Form.ContainsKey("ReturnURL"))
                    {
                        return Redirect(Request.Form["ReturnURL"]!);
                    }
                    return RedirectToAction("Index", "Personnage");
                }
            }
            Response.StatusCode = 403;
            ViewData["ValidateMessage"] = "Wrong email or password.";
            return View();
        }

        public IActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Inscription(Utilisateur utilisateur)
        {
            string query = "SELECT * FROM Utilisateur WHERE EmailUtilisateur = @EmailUtilisateur";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                List<Utilisateur> utilisateurFromDB = connexion.Query<Utilisateur>(query, new { EmailUtilisateur = utilisateur.EmailUtilisateur }).ToList();

                if (utilisateurFromDB.Count > 0)
                {
                    ViewData["ValidateMessage"] = "email already used";
                    return View();
                }
                else
                {
                    string insertQuery = "INSERT INTO Utilisateur (NomUtilisateur,EmailUtilisateur,Password) VALUES ('toto',@EmailUtilisateur,@Password)";

                    string HashedPassword = BC.HashPassword(utilisateur.Password);

                    int RowsAffected = connexion.Execute(insertQuery, new { EmailUtilisateur = utilisateur.EmailUtilisateur, Password = HashedPassword });
                    if (RowsAffected == 1)
                    {
                        ViewData["ValidateMessage"] = "Your subscribtion is done. Please log in with your credentials.";
                        return View("Login");

                    }
                    else
                    {
                        ViewData["ValidateMessage"] = "Error during the signin process, please try again.";
                        return View();
                    }
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }
    }
}
