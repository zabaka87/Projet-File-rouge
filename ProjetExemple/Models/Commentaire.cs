using System.ComponentModel.DataAnnotations;

namespace MagicBook.Models // Définit le namespace MagicBook.Models pour les modèles de l'application.
{
    // Définit la classe Commentaire pour représenter un commentaire sur un livre.
    public class Commentaire
    {
        public int IdCommentaire { get; set; }

        [Required(ErrorMessage = "Le champ commentaire est requis.")]
        public string commentaire { get; set; } // Déclare une propriété pour l'identifiant du commentaire.

        public DateTime DateCommentaire { get; set; }

        public int? IdUtilisateur { get; set; } // Déclare une propriété pour l'identifiant de l'utilisateur qui a fait le commentaire.

      
    }
}
