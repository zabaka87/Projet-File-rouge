using System.ComponentModel.DataAnnotations; // Importe les attributs de validation des données.

namespace MagicBook.Models // Définit le namespace MagicBook.Models pour les modèles de l'application.
{
    // Définit la classe genres pour représenter une catégorie de livre.
    public class genres
    {
        [Display(Name = "IdCategorie")] // Spécifie le nom à afficher pour l'identifiant de la catégorie.
        public int IdCategorie { get; set; } // Déclare une propriété pour l'identifiant de la catégorie.

        [Display(Name = "Genre")] // Spécifie le nom à afficher pour le nom de la catégorie.
        public string? NomCategorie { get; set; } // Déclare une propriété pour le nom de la catégorie.
    }
}
