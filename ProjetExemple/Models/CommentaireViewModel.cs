namespace MagicBook.Models // Définit le namespace MagicBook.Models pour les modèles de l'application.
{
    // Définit la classe CommentaireViewModel pour représenter les détails d'un commentaire dans la vue.
    public class CommentaireViewModel
    {
        public string Commentaire { get; set; } // Déclare une propriété pour le contenu du commentaire.

        public DateTime DateCommentaire { get; set; } // Déclare une propriété pour la date et l'heure du commentaire.

        public string PseudoUtilisateur { get; set; }// Déclare une propriété pour le pseudo de l'utilisateur qui a fait le commentaire.
        //public string ISBN { get; set; }
    }
}
