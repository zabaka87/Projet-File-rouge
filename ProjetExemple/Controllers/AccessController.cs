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
            string query = "SELECT NomUtilisateur, EmailUtilisateur, Password FROM Utilisateur WHERE EmailUtilisateur = @EmailUtilisateur";

            using (var connexion = new MySqlConnection(_connexionString))
            {
                var utilisateurFromDB = await connexion.QueryFirstOrDefaultAsync<Utilisateur>(query, new { EmailUtilisateur = utilisateur.EmailUtilisateur });

                if (utilisateurFromDB != null && BC.Verify(utilisateur.Password, utilisateurFromDB.Password))
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, utilisateurFromDB.EmailUtilisateur),
                        new Claim(ClaimTypes.Name, utilisateurFromDB.NomUtilisateur)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = utilisateur.KeepLoggedIn
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    if (Request.Form.ContainsKey("ReturnURL"))
                    {
                        return Redirect(Request.Form["ReturnURL"]);
                    }

                    return RedirectToAction("Index", "Utilisateur");
                }
            }

            Response.StatusCode = 403;
            ViewData["ValidateMessage"] = "Adresse e-mail ou mot de passe incorrect.";
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
                    string insertQuery = "INSERT INTO Utilisateur (NomUtilisateur,PrenomUtilisateur,PseudoUtilisateur,EmailUtilisateur,Adresse1,Adresse2,CodePostal,Ville,DateInscription,Password) VALUES (@NomUtilisateur,@PrenomUtilisateur,@PseudoUtilisateur,@EmailUtilisateur,@Adresse1,@Adresse2,@CodePostal,@Ville,NOW(),@Password)";

                    string HashedPassword = BC.HashPassword(utilisateur.Password);

                    int RowsAffected = connexion.Execute(insertQuery, new { NomUtilisateur = utilisateur.NomUtilisateur, PrenomUtilisateur = utilisateur.PrenomUtilisateur, PseudoUtilisateur = utilisateur.PseudoUtilisateur, EmailUtilisateur = utilisateur.EmailUtilisateur, Adresse1 = utilisateur.Adresse1, Adresse2 = utilisateur.Adresse2, CodePostal = utilisateur.CodePostal, Ville = utilisateur.Ville, DateInscription = utilisateur.DateInscription, Password = HashedPassword });
                    if (RowsAffected == 1)
                    {
                        ViewData["ValidateMessage"] = "Votre compte a été cree avec succe , veuiller s'identifier.";
                        return View("Login");

                    }
                    else
                    {
                        ViewData["ValidateMessage"] = "Un erreur a été détécter durant votre inscription,veuiller resseyer.";
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
