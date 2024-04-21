using Microsoft.AspNetCore.Mvc; // Importe les classes et les interfaces nécessaires pour créer des contrôleurs MVC.
using MagicBook.Models; // Importe les modèles nécessaires pour interagir avec les données de l'application.
using Dapper; // Importe Dapper, un micro-ORM qui permet de mapper les résultats des requêtes SQL aux objets .NET.
using MySql.Data.MySqlClient; // Importe le fournisseur de données MySQL pour établir une connexion à la base de données MySQL.
using Microsoft.AspNetCore.Authorization; // Importe les fonctionnalités d'autorisation pour restreindre l'accès aux ressources.
using Microsoft.AspNetCore.Mvc.Rendering; // Importe les classes pour générer des éléments de formulaire HTML.
using Microsoft.AspNetCore.Cors; // Importe les fonctionnalités de gestion des autorisations Cross-Origin Resource Sharing (CORS).
using System.Diagnostics; // Importe la classe Debug pour les informations de débogage.
using System.Security.Claims; // Importe la classe Claims pour représenter les revendications d'authentification et d'autorisation.


namespace MagicBook.Controllers
{
    [Authorize] // Autorisation requise pour accéder à ce contrôleur
    public class LivreController : Controller
    {
        private readonly string _connexionString;

        // Constructeur du contrôleur qui initialise la chaîne de connexion à la base de données
        public LivreController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MagicBook")!;
        }

        // Action pour afficher les détails d'un livre
        public IActionResult Detail(string id)
        {
            // Requête SQL pour récupérer les détails d'un livre avec l'ISBN spécifié
            string queryLivre = "SELECT * FROM Livre JOIN Editeur ON Livre.IdEditeur = Editeur.IdEditeur JOIN Categorie ON Livre.IdCategorie = Categorie.IdCategorie WHERE Livre.ISBN = @id";
            String queryCommentaire = "select * from Commentaire join Utilisateur on Commentaire.IdUtilisateur=Utilisateur.IdUtilisateur Left join CommentaireLivre on Commentaire.IdCommentaire=CommentaireLivre.IdCommentaire where CommentaireLivre.ISBN= @id Limit 4";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                try
                {
                    // Exécution de la requête SQL et récupération des détails du livre
                    LivreViewModel livre = connexion.QuerySingle<LivreViewModel>(queryLivre, new { id = id });
                    List<CommentaireViewModel> commentaires =connexion.Query<CommentaireViewModel>(queryCommentaire, new { id = id }).ToList();

                    // Ajout des commentaires récupérés dans le ViewModel
                    if (commentaires != null)
                    {
                        livre.Commentaire = commentaires;
                    }
                    else
                    {
                        livre.Commentaire = new List<CommentaireViewModel>();
                    }




                    // Affichage de la vue avec les détails du livre
                    return View(livre);
                }
                catch (InvalidOperationException)
                {
                    // Redirection vers une page d'erreur si le livre n'est pas trouvé
                    return RedirectToAction("NotFound", "Errors");
                }
            }
        }

        // Action pour afficher le formulaire d'ajout d'un nouveau livre (GET)
        [HttpGet]
        public IActionResult Ajouter()
        {
            // Affichage de la vue avec le formulaire d'ajout de livre
            return View(GenerateAjoutLivreViewModel());
        }

        // Action pour traiter la soumission du formulaire d'ajout d'un nouveau livre (POST)
        [HttpPost]
        public IActionResult Ajouter(Livre livre)
        {
            // Validation du modèle

            // Vérification de la non-utilisation de la clé primaire (ISBN)
            string ClePremaireQuery = "SELECT COUNT(*) FROM Livre WHERE ISBN=@ISBN";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                int res = connexion.ExecuteScalar<int>(ClePremaireQuery, new { ISBN = livre.ISBN });
                if (res > 0)
                {
                    // Affichage d'un message d'erreur si l'ISBN est déjà utilisé
                    ViewData["ValidateMessage"] = "Ce numéro ISBN est déjà utilisé, veuillez en choisir un autre.";
                    return View(GenerateAjoutLivreViewModel());
                }
            }

            // Requête SQL pour ajouter un nouveau livre à la base de données
            string InsertQuery = "INSERT INTO Livre(ISBN,TitreLivre,ResumeLivre,DatePublicationLivre,IdEditeur,IdCategorie,image) VALUES (@ISBN,@TitreLivre,@ResumeLivre,@DatePublicationLivre,@IdEditeur,@IdCategorie,@image)";

            string? filePath = null;

            // Traitement de l'upload de l'image du livre
            if (Request.Form.Files.Count == 1 && Request.Form.Files[0].Length > 0)
            {
                // Enregistrement de l'image dans le dossier wwwroot/images/livre
                filePath = Path.Combine("/images/livre/",
                    Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(Request.Form.Files[0].FileName)).ToString();

                using (var stream = System.IO.File.Create("wwwroot" + filePath))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }

                // Mise à jour du chemin de l'image dans l'objet livre
                livre.image = filePath;
            }

            using (var connexion = new MySqlConnection(_connexionString))
            {
                try
                {
                    // Exécution de la requête SQL pour ajouter le livre à la base de données
                    int rowsCreated = connexion.Execute(InsertQuery, livre);
                    if (rowsCreated == 0)
                    {
                        // Suppression de l'image si l'ajout du livre a échoué
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
                    // Suppression de l'image si l'ajout du livre a échoué en raison d'une exception MySQL
                    if (filePath != null)
                    {
                        System.IO.File.Delete("wwwroot" + filePath);
                    }
                    ViewData["ValidateMessage"] = "ERREUR DE CHARGEMENT VEUILLIER RESSEYER";
                }
            }

            // Affichage de la vue avec le formulaire d'ajout de livre
            return View(GenerateAjoutLivreViewModel());
        }

        // Méthode pour générer le ViewModel utilisé par le formulaire d'ajout de livre
        private AjoutLivreViewModel GenerateAjoutLivreViewModel()
        {
            // Récupération de toutes les catégories et tous les éditeurs depuis la base de données
            string selectCategorieQuery = "SELECT IdCategorie, NomCategorie FROM Categorie;";
            string selectEditeursQuery = "SELECT IdEditeur, NomEditeur FROM Editeur;";
            AjoutLivreViewModel viewModel = new AjoutLivreViewModel();

            using (var connexion = new MySqlConnection(_connexionString))
            {
                // Récupération des catégories et ajout dans le ViewModel
                List<genres> categories = connexion.Query<genres>(selectCategorieQuery).ToList();
                foreach (genres categorie in categories)
                {
                    viewModel.categories.Add(new SelectListItem { Text = categorie.NomCategorie, Value = categorie.IdCategorie.ToString() });
                }

                // Récupération des éditeurs et ajout dans le ViewModel
                List<Editeur> editeurs = connexion.Query<Editeur>(selectEditeursQuery).ToList();
                foreach (Editeur editeur in editeurs)
                {
                    viewModel.editeurs.Add(new SelectListItem { Text = editeur.NomEditeur, Value = editeur.IdEditeur.ToString() });
                }
            }

            return viewModel;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult AjouterCommentaire(Commentaire AjoutCom)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    string queryInsertCommentaire = "INSERT INTO Commentaire (commentaire, DateCommentaire, IdUtilisateur) " +
        //                                    "VALUES (@Commentaire, NOW(), @IdUtilisateur);";

        //    string queryInsertLien = "INSERT INTO CommentaireLivre (IdCommentaire,ISBN) " +
        //        "VALUES (LAST_INSERT_ID(),@ISBN);";
        //    using (var connexion = new MySqlConnection(_connexionString))
        //    {
        //        try
        //        {
        //            connexion.Open();
        //            using (var transaction = connexion.BeginTransaction())
        //            {
        //                string idUtilis = User.FindFirstValue(ClaimTypes.SerialNumber);
        //                // Insertion du commentaire
        //                connexion.Execute(queryInsertCommentaire, new { Commentaire = AjoutCom.commentaire, IdUtilisateur = User.FindFirstValue(ClaimTypes.SerialNumber) }, transaction);

        //                // Récupération de l'ID du commentaire inséré
        //                var idCommentaire = connexion.ExecuteScalar<int>("SELECT LAST_INSERT_ID();", transaction);

        //                // Insertion du lien entre le commentaire et le livre
        //                connexion.Execute(queryInsertLien, new { idCommentaire, ISBN = AjoutCom.ISBN }, transaction);

        //                transaction.Commit();
        //                TempData["ValidateMessage"] = "Le commentaire a été ajouté avec succès.";
        //            }
                      

        //        }
        //        catch (MySqlException)
        //        {
        //            TempData["ValidateMessage"] = "Il y a eu un problème lors de l'ajout du commentaire.";
        //        }
        //    }
        //    //}
        //    //else
        //    //{
        //    //    TempData["ValidateMessage"] = "Les données fournies ne sont pas correctes.";
        //    //}

        //    return RedirectToAction("Detail", new { id = AjoutCom.ISBN });
        //}










        //// Action pour afficher les détails d'un livre
        //public IActionResult listCommentaire(string id)
        //{
        //    // Requête SQL pour récupérer les détails d'un livre avec l'ISBN spécifié
        //    string query = "SELECT * FROM Commentaire JOIN CommentaireLivre ON Commentaire.IdCommentaire = CommentaireLivre.IdCommentaire JOIN Livre ON Livre.ISBN = CommentaireLivre.ISBN WHERE Livre.ISBN = @id";

        //    using (var connexion = new MySqlConnection(_connexionString))
        //    {
        //        try
        //        {
        //            // Exécution de la requête SQL et récupération des détails du livre
        //            LivreViewModel livre = connexion.QuerySingle<LivreViewModel>(query, new { id = id });

        //            // Todo: Remplir la liste de commentaires du livre

        //            // Affichage de la vue avec les détails du livre
        //            return View(livre);
        //        }
        //        catch (InvalidOperationException)
        //        {
        //            // Redirection vers une page d'erreur si le livre n'est pas trouvé
        //            return RedirectToAction("NotFound", "Errors");
        //        }
        //    }
        //}
    }
}






