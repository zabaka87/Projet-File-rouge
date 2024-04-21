using Microsoft.AspNetCore.Authentication; // Importe les fonctionnalités d'authentification.
using Microsoft.AspNetCore.Authentication.Cookies; // Importe le gestionnaire d'authentification par cookies.
using Microsoft.AspNetCore.Mvc; // Importe les classes et les interfaces nécessaires pour créer des contrôleurs MVC.
using MySql.Data.MySqlClient; // Importe le fournisseur de données MySQL pour établir une connexion à la base de données MySQL.
using MagicBook.Models; // Importe les modèles nécessaires pour interagir avec les données de l'application.
using System.Security.Claims; // Importe la classe Claims pour représenter les revendications d'authentification et d'autorisation.
using Dapper; // Importe Dapper, un micro-ORM qui permet de mapper les résultats des requêtes SQL aux objets .NET.
using BC = BCrypt.Net.BCrypt; // Alias pour la classe BCrypt pour le hachage des mots de passe.
using Microsoft.AspNetCore.Authorization; // Importe les fonctionnalités d'autorisation pour restreindre l'accès aux ressources.

namespace MagicBook.Controllers // Définit le namespace MagicBook.Controllers pour le contrôleur AccessController.
{
    public class AccessController : Controller // Définit la classe AccessController qui hérite de la classe Controller.
    {
        private readonly string _connexionString; // Déclare une variable de chaîne pour la connexion à la base de données.

        // Constructeur de la classe AccessController qui prend IConfiguration comme paramètre pour obtenir la chaîne de connexion.
        public AccessController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MagicBook")!; // Initialise la chaîne de connexion.
        }

        // Action Login qui affiche la vue de connexion.
        public IActionResult Login()
        {
            ClaimsPrincipal User = HttpContext.User; // Récupère l'utilisateur actuel.
            if (User.Identity!.IsAuthenticated) // Vérifie si l'utilisateur est déjà authentifié.
            {
                return RedirectToAction("Index", "Home"); // Redirige vers la page d'accueil de l'utilisateur.
            }
            return View(); // Affiche la vue de connexion.
        }

        // Action Login qui gère la soumission du formulaire de connexion.
        [HttpPost]
        public async Task<IActionResult> Login(Utilisateur utilisateur)
        {
            string query = "SELECT NomUtilisateur, EmailUtilisateur, Password FROM Utilisateur WHERE EmailUtilisateur = @EmailUtilisateur";

            using (var connexion = new MySqlConnection(_connexionString))
            {
                var utilisateurFromDB = await connexion.QueryFirstOrDefaultAsync<Utilisateur>(query, new { EmailUtilisateur = utilisateur.EmailUtilisateur });

                // Vérifie si l'utilisateur existe dans la base de données et si le mot de passe est correct.
                if (utilisateurFromDB != null && BC.Verify(utilisateur.Password, utilisateurFromDB.Password))
                {
                    // Crée les revendications de l'utilisateur.
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, utilisateurFromDB.EmailUtilisateur),
                        new Claim(ClaimTypes.Name, utilisateurFromDB.NomUtilisateur)
                    };

                    // Crée une identité contenant les revendications de l'utilisateur.
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Définit les propriétés d'authentification.
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = utilisateur.KeepLoggedIn
                    };

                    // Connecte l'utilisateur en créant un cookie d'authentification.
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Redirige l'utilisateur vers la page d'accueil.
                    if (Request.Form.ContainsKey("ReturnURL"))
                    {
                        return Redirect(Request.Form["ReturnURL"]);
                    }
                    return RedirectToAction("Index", "Utilisateur");
                }
            }

            // Si l'authentification échoue, affiche un message d'erreur.
            Response.StatusCode = 403;
            ViewData["ValidateMessage"] = "Adresse e-mail ou mot de passe incorrect.";
            return View();
        }

        // Action Inscription qui affiche la vue d'inscription.
        public IActionResult Inscription()
        {
            return View();
        }

        // Action Inscription qui gère la soumission du formulaire d'inscription.
        [HttpPost]
        public IActionResult Inscription(Utilisateur utilisateur)
        {
            string query = "SELECT * FROM Utilisateur WHERE EmailUtilisateur = @EmailUtilisateur";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                // Vérifie si l'adresse e-mail est déjà utilisée.
                List<Utilisateur> utilisateurFromDB = connexion.Query<Utilisateur>(query, new { EmailUtilisateur = utilisateur.EmailUtilisateur }).ToList();

                if (utilisateurFromDB.Count > 0)
                {
                    ViewData["ValidateMessage"] = "email already used"; // Affiche un message d'erreur si l'adresse e-mail est déjà utilisée.
                    return View();
                }
                else
                {
                    // Requête SQL pour insérer un nouvel utilisateur dans la base de données.
                    string insertQuery = "INSERT INTO Utilisateur (NomUtilisateur,PrenomUtilisateur,PseudoUtilisateur,EmailUtilisateur,Adresse1,Adresse2,CodePostal,Ville,DateInscription,Password) VALUES (@NomUtilisateur,@PrenomUtilisateur,@PseudoUtilisateur,@EmailUtilisateur,@Adresse1,@Adresse2,@CodePostal,@Ville,NOW(),@Password)";

                    // Hache le mot de passe de l'utilisateur avant de l'insérer dans la base de données.
                    string HashedPassword = BC.HashPassword(utilisateur.Password);

                    // Exécute la requête SQL pour insérer l'utilisateur dans la base de données.
                    int RowsAffected = connexion.Execute(insertQuery, new { NomUtilisateur = utilisateur.NomUtilisateur, PrenomUtilisateur = utilisateur.PrenomUtilisateur, PseudoUtilisateur = utilisateur.PseudoUtilisateur, EmailUtilisateur = utilisateur.EmailUtilisateur, Adresse1 = utilisateur.Adresse1, Adresse2 = utilisateur.Adresse2, CodePostal = utilisateur.CodePostal, Ville = utilisateur.Ville, DateInscription = utilisateur.DateInscription, Password = HashedPassword });

                    // Affiche un message de succès ou d'erreur selon le résultat de l'inscription.
                    if (RowsAffected == 1)
                    {
                        ViewData["ValidateMessage"] = "Votre compte a été créé avec succès, veuillez vous identifier.";
                        return View("Login");
                    }
                    else
                    {
                        ViewData["ValidateMessage"] = "Une erreur a été détectée lors de votre inscription, veuillez réessayer.";
                        return View();
                    }
                }
            }
        }

        // Action LogOut qui déconnecte l'utilisateur en supprimant le cookie d'authentification.
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }
    }
}
