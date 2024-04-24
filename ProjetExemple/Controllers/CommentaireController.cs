using Dapper;
using MagicBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Claims;

namespace MagicBook.Controllers
{
    [Authorize]
    public class CommentaireController : Controller
    {


        private readonly string _connexionString;

        // Constructeur du contrôleur qui initialise la chaîne de connexion à la base de données
        public CommentaireController(IConfiguration configuration)
        {
            _connexionString = configuration.GetConnectionString("MagicBook")!;
        }




        public IActionResult Index(string id)
        {
            ViewData["ISBN"] = id;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AjouterCommentaire(Commentaire AjoutCom)
        {
            //if (ModelState.IsValid)
            //{
            string queryInsertCommentaire = "INSERT INTO Commentaire (commentaire, DateCommentaire, IdUtilisateur) " +
                                            "VALUES (@Commentaire, NOW(), @IdUtilisateur);";

            string queryInsertLien = "INSERT INTO CommentaireLivre (IdCommentaire,ISBN) " +
                "VALUES (LAST_INSERT_ID(),@ISBN);";
            using (var connexion = new MySqlConnection(_connexionString))
            {
                try
                {
                    connexion.Open();
                    using (var transaction = connexion.BeginTransaction())
                    {
                        string idUtilis = User.FindFirstValue(ClaimTypes.SerialNumber);
                        // Insertion du commentaire
                        connexion.Execute(queryInsertCommentaire, new { Commentaire = AjoutCom.commentaire, IdUtilisateur = idUtilis }, transaction);

                        // Récupération de l'ID du commentaire inséré
                        var idCommentaire = connexion.ExecuteScalar<int>("SELECT LAST_INSERT_ID();", transaction);

                        // Insertion du lien entre le commentaire et le livre
                        connexion.Execute(queryInsertLien, new { idCommentaire, ISBN = AjoutCom.ISBN }, transaction);

                        transaction.Commit();
                        TempData["ValidateMessage"] = "Le commentaire a été ajouté avec succès.";
                    }


                }
                catch (MySqlException)
                {
                    TempData["ValidateMessage"] = "Il y a eu un problème lors de l'ajout du commentaire.";
                }
            }
            //}
            //else
            //{
            //    TempData["ValidateMessage"] = "Les données fournies ne sont pas correctes.";
            //}

            return RedirectToAction("Detail", new { id = AjoutCom.ISBN });
        }

    }


}
