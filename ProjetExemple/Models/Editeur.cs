using System.ComponentModel.DataAnnotations; // Importe les attributs de validation des données.

namespace MagicBook.Models // Définit le namespace MagicBook.Models pour les modèles de l'application.
{
    // Définit la classe Editeur pour représenter un éditeur de livre.
    public class Editeur
    {
        [Display(Name = "IdEditeur")] // Spécifie le nom à afficher pour l'identifiant de l'éditeur.
        public int IdEditeur { get; set; } // Déclare une propriété pour l'identifiant de l'éditeur.

        [Display(Name = "Editeur")] // Spécifie le nom à afficher pour le nom de l'éditeur.
        public string? NomEditeur { get; set; } // Déclare une propriété pour le nom de l'éditeur.
    }
}
