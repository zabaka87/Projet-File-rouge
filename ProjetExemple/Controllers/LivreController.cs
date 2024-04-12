using Microsoft.AspNetCore.Mvc;
using MagicBook.Models;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Cors;
using System.Diagnostics;
using System.Security.Claims;


namespace MagicBook.Controllers
{
    [Authorize]
    public class LivreController : Controller
    {


        private readonly string _connexionString;
        public LivreController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MagicBook")!;
        }

        public IActionResult Detail(string id)
        {
            string query = "Select * from Livre join Editeur on Livre.IdEditeur=Editeur.IdEditeur join Categorie on Livre.IdCategorie=Categorie.IdCategorie where ISBN =@id";

            string QueryCommentaires = "";

            using (var connexion = new MySqlConnection(_connexionString))
            {
                try
                {
                    LivreViewModel livre = connexion.QuerySingle<LivreViewModel>(query, new { id = id });

                    // todo : rempli la liste de commentaires du livre


                    return View(livre);
                }
                catch (InvalidOperationException)
                {
                    return RedirectToAction("NotFound", "Errors");
                }
            }
        }
        //HttpGet permet d'afficher sans pouvoir modifier les parametres
        [HttpGet]
        public IActionResult Ajouter()
        {
            return View(GenerateAjoutLivreViewModel());
        }

        
       
        [HttpPost]
        public IActionResult Ajouter(Livre livre)
        {
            //validation du modèle

            // vérification de la non-utilisation de la clé primaire
            string ClePremaireQuery = "SELECT COUNT(*) FROM Livre WHERE ISBN=@ISBN";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                int res = connexion.ExecuteScalar<int>(ClePremaireQuery, new { ISBN = livre.ISBN });
                if (res > 0)
                {
                    ViewData["ValidateMessage"] = "Ce numéro ISBN est déjà utilisé, veuillez en choisir un autre.";
                    return View(GenerateAjoutLivreViewModel());
                }
            }


            //requête SQL permettant d'ajouter un Livre à la base de données
            string InsertQuery = "INSERT INTO Livre(ISBN,TitreLivre,ResumeLivre,DatePublicationLivre,IdEditeur,IdCategorie,image) VALUES (@ISBN,@TitreLivre,@ResumeLivre,@DatePublicationLivre,@IdEditeur,@IdCategorie,@image)";

           
            string? filePath = null;
            if (Request.Form.Files.Count == 1 && Request.Form.Files[0].Length > 0)
            {
                filePath = Path.Combine("/images/livre/",
                    Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(Request.Form.Files[0].FileName)).ToString();

                using (var stream = System.IO.File.Create("wwwroot" + filePath))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                livre.image = filePath;
            }

            
            using (var connexion = new MySqlConnection(_connexionString))
            {
                try
                {
                    int rowsCreated = connexion.Execute(InsertQuery, livre);
                    if (rowsCreated == 0)
                    {
                        
                        if (filePath != null)
                        {
                            System.IO.File.Delete("wwwroot" + filePath);
                        }
                        ViewData["ValidateMessage"] = "ERREUR DE CHARGEMENT VEUILLIER RESSEYER";
                    }
                    else
                    {
                        ViewData["ValidateMessage"] = "Book created !";
                    }
                }
                catch (MySqlException)
                {
                    //supprimer l'image si elle a été créé
                    if (filePath != null)
                    {
                        System.IO.File.Delete("wwwroot" + filePath);
                    }
                    ViewData["ValidateMessage"] = "ERREUR DE CHARGEMENT VEUILLIER RESSEYER";
                }
            }

            return View(GenerateAjoutLivreViewModel());

        }

        private AjoutLivreViewModel GenerateAjoutLivreViewModel()
        {
            // récupération de toutes les races
            string selectCategorieQuery = "SELECT IdCategorie, NomCategorie FROM Categorie;";
            string selectEditeursQuery = "SELECT IdEditeur, NomEditeur FROM Editeur;";
            AjoutLivreViewModel viewModel = new AjoutLivreViewModel();
            using (var connexion = new MySqlConnection(_connexionString))
            {
                List<genres> categories = connexion.Query<genres>(selectCategorieQuery).ToList();
                foreach (genres categorie in categories)
                {
                    viewModel.categories.Add(new SelectListItem { Text = categorie.NomCategorie, Value = categorie.IdCategorie.ToString() });
                }
                List<Editeur> editeurs = connexion.Query<Editeur>(selectEditeursQuery).ToList();
                foreach (Editeur editeur in editeurs)
                {
                    viewModel.editeurs.Add(new SelectListItem { Text = editeur.NomEditeur, Value = editeur.IdEditeur.ToString() });
                }

            }
            return viewModel;
        }

       
        [HttpPost]
        public IActionResult Commentaire(Commentaire commentaire)
        {
            //validation du modèle

            // vérification de la non-utilisation de la clé primaire
            string ClePremaireQuery = "SELECT COUNT(*) FROM Commentaire WHERE IdCommentaire=@IdCommentaire";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                int res = connexion.ExecuteScalar<int>(ClePremaireQuery, new { ISBN = livre.ISBN });
                if (res > 0)
                {
                    ViewData["ValidateMessage"] = "Ce numéro ISBN est déjà utilisé, veuillez en choisir un autre.";
                    return View(GenerateAjoutLivreViewModel());
                }
            }

            //vérification de l'existance du commentaire donné par l'utilisateur dans la bdd

            //requête SQL permettant d'ajouter un commentaire à la base de données
            string InsertQuery = "INSERT INTO Commentaire(IdCommentaire,Commentaire,DateCommentaire,IdUtilisateur,ISBN) VALUES (@IdCommentaire,@Commentaire,@DateCommentaire,@IdUtilisateur,@ISBN)";


            string? filePath = null;
            if (Request.Form.Files.Count == 1 && Request.Form.Files[0].Length > 0)
            {
                filePath = Path.Combine("/images/livre/",
                    Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(Request.Form.Files[0].FileName)).ToString();

                using (var stream = System.IO.File.Create("wwwroot" + filePath))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                livre.image = filePath;
            }


            using (var connexion = new MySqlConnection(_connexionString))
            {
                try
                {
                    int rowsCreated = connexion.Execute(InsertQuery, livre);
                    if (rowsCreated == 0)
                    {

                        if (filePath != null)
                        {
                            System.IO.File.Delete("wwwroot" + filePath);
                        }
                        ViewData["ValidateMessage"] = "There was a problem during the process, please try again";
                    }
                    else
                    {
                        ViewData["ValidateMessage"] = "Book created !";
                    }
                }
                catch (MySqlException)
                {
                    //supprimer l'image si elle a été créé
                    if (filePath != null)
                    {
                        System.IO.File.Delete("wwwroot" + filePath);
                    }
                    ViewData["ValidateMessage"] = "There was a problem during the process, please try again";
                }
            }

            return View(GenerateAjoutLivreViewModel());

        }
    }
}
