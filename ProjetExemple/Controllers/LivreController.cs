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
                    List<CommentaireViewModel> commentaires = connexion.Query<CommentaireViewModel>(queryCommentaire, new { id = id }).ToList();

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

        [HttpGet]
        public IActionResult Rechercher(string ThemeRecherche)
        {

            if (ThemeRecherche == null)
            {
                return RedirectToAction("Home", "Accueil");
            }


            //cette requête sélectionne tous les livres dont l'ISBN ou le titre contient la chaîne de recherche.
            string QueryRecherche = "SELECT * FROM Livre  join Categorie on Livre.Idcategorie = Categorie.IdCategorie join Editeur on Livre.IdEditeur = Editeur.IdEditeur where ISBN like '%" + @ThemeRecherche + "%'  or TitreLivre like '%" + @ThemeRecherche + "%'";

            //cette requête sélectionne tous les auteurs dont le nom ou le prénom contient la chaîne de recherche.
            string queryAuteur = "select * from Auteur a where a.NomAuteur like '%" + @ThemeRecherche + "%' OR a.PrenomAuteur like '%" + @ThemeRecherche + "%' OR CONCAT(a.PrenomAuteur, ' ', a.NomAuteur) like '%" + @ThemeRecherche + "%'";

            //cette requête sélectionne tous les livres écrits par un auteur donné.
            string queryEstEcrit = "select * from Livre_Auteur e join Livre l on e.ISBN=l.ISBN where e.IdAuteur = @IdAuteur";

            // Creation d un objet ListeLivreViewModel qui sera utilisé pour stocker les resultats de la recherche
            ListeLivresViewModel ListeLivresRecherche = new ListeLivresViewModel();

            //Ouverture d une connexion a la base de donnée MySQL 
            using (var connexion = new MySqlConnection(_connexionString))
            {
                // Execution de la requete 'QueryRecherche' et stockage du resultat dans la proprieté 'Livres' de l objet ListLivreViewModel
                ListeLivresRecherche.livres = connexion.Query<LivreViewModel>(QueryRecherche, new { ThemeRecherche = ThemeRecherche }).ToList();

                // Execution de la requete 'QueryAuteur' et stockage du resultat dans une liste d auteurs contenue dans un objet LivreViewModel
                List<AuteurViewModel> Auteurs = connexion.Query<AuteurViewModel>(queryAuteur, new { ThemeRecherche = ThemeRecherche }).ToList();


                // Pour chaque auteur de la liste, execution de la requete 'estEcrit' et ajoute les resultats a la proprieté 'Livres' de l objet 'ListeLivreViewModel'
                foreach (AuteurViewModel auteur in Auteurs)
                {

                    ListeLivresRecherche.livres.AddRange(connexion.Query<LivreViewModel>(queryEstEcrit, new { IdAuteur = auteur.IdAuteur }).ToList());
                }

                // Pour chaque livre dansl a propriete de l objet 'ListeLivreViewModel', execution d une requete SQL qui selectionne les noms et les prenoms des auteurs du livre et stocke le resultat dans la proprieté 'auteurs' du livre
                foreach (LivreViewModel livre in ListeLivresRecherche.livres)
                {
                    string QueryAuteur = "select Auteur.NomAuteur , Auteur.PrenomAuteur from Auteur join Livre_Auteur on Auteur.IdAuteur = Livre_Auteur.IdAuteur join Livre on Livre_Auteur.ISBN = Livre.ISBN where Livre.ISBN = @ISBN";

                    livre.auteurs = connexion.Query<AuteurViewModel>(QueryAuteur, new { ISBN = livre.ISBN }).ToList();

                }

                // Cette requete selectionne les Nom De categorie de livre de la base de donnée
                string selectQueryCategorie = "select distinct IdCategorie, NomCategorie from Categorie;";

                // Execution de la requete 'selectQueryCategorie' ee stockage des resultats dans une liste de 'SelectListItem ' qui sera utilisée pour afficher les categorie dans  une liste deroulante dans la vue
                List<genres> Categories = connexion.Query<genres>(selectQueryCategorie).ToList();
                foreach (genres categorie in Categories)
                {
                    ListeLivresRecherche.Categories.Add(new SelectListItem { Text = genres.IdCategorie, Value = genres.NomCategorie.ToString() });
                }

                if (ListeLivresRecherche.livres == null || !ListeLivresRecherche.livres.Any())
                {
                    ViewData["ValidateMessage"] = "Aucun livre n'a été trouvé.";
                }

                // retourne la vue 'PageDAccueil' avec l objet 'ListeLivresViewModel' comme modele
                return Json(ListeLivresRecherche.livres);
            }
        }
    }
}















