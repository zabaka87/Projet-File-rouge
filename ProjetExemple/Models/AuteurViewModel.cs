using System.ComponentModel.DataAnnotations;
namespace MagicBook.Models
{
    public class AuteurViewModel
    {
        // Définit la classe Auteur pour représenter un auteur de livre.
        public class Editeur
        {
            [Display(Name = "IdAuteur")] // Spécifie le nom à afficher pour l'identifiant de l'auteur.
            public int IdAuteur { get; set; } // Déclare une propriété pour l'identifiant de l'auteur.

            [Display(Name = "NomAuteur")] // Spécifie le nom à afficher pour le nom de l'auteur.
            public string? NomAuteur { get; set; } // Déclare une propriété pour le nom de l'auteur.

            [Display(Name = "PrenomAuteur")] // Spécifie le nom à afficher pour le nom de l'auteur.
            public string? PrenomAuteur { get; set; } // Déclare une propriété pour le nom de l'auteur.
        }
    }
}
