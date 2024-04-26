using System.ComponentModel.DataAnnotations; // Importe les attributs de validation des données.

namespace MagicBook.Models // Définit le namespace MagicBook.Models pour les modèles de l'application.
{
    // Définit la classe LivreViewModel pour représenter les détails d'un livre dans la vue.
    public class LivreViewModel
    {
        [Display(Name = "ISBN")] // Spécifie le nom à afficher pour l'ISBN du livre.
        public string ISBN { get; set; } // Déclare une propriété pour l'ISBN du livre.

        [Display(Name = "Titre")] // Spécifie le nom à afficher pour le titre du livre.
        public string? TitreLivre { get; set; } // Déclare une propriété pour le titre du livre.

        [Display(Name = "Resumer")] // Spécifie le nom à afficher pour le résumé du livre.
        public string? ResumeLivre { get; set; } // Déclare une propriété pour le résumé du livre.

        [Display(Name = "Date Publication")] // Spécifie le nom à afficher pour la date de publication du livre.
        public DateTime? DatePublicationLivre { get; set; } // Déclare une propriété pour la date de publication du livre.

        [Display(Name = "Nom Editeur")] // Spécifie le nom à afficher pour le nom de l'éditeur du livre.
        public string NomEditeur { get; set; } // Déclare une propriété pour le nom de l'éditeur du livre.

        [Display(Name = "Genre")] // Spécifie le nom à afficher pour le genre du livre.
        public string NomCategorie { get; set; } // Déclare une propriété pour le genre du livre.

        [Display(Name = "Image")] // Spécifie le nom à afficher pour l'image du livre.
        public string Image { get; set; } // Déclare une propriété pour l'image du livre.

        public List<CommentaireViewModel> Commentaire { get; set; } = new List<CommentaireViewModel>(); // Déclare une propriété pour la liste des commentaires du livre.
        public List<AuteurViewModel> auteurs { get; internal set; } = new List<AuteurViewModel>();
    }
}
