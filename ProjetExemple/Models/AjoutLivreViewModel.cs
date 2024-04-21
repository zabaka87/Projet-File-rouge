using Microsoft.AspNetCore.Mvc.Rendering; // Importe les classes nécessaires pour créer des listes déroulantes dans les vues.
using System.ComponentModel.DataAnnotations; // Importe les attributs de validation des données.

namespace MagicBook.Models // Définit le namespace MagicBook.Models pour les modèles de l'application.
{
    // Définit la classe AjoutLivreViewModel pour afficher le formulaire d'ajout de livre avec les listes déroulantes des éditeurs et des catégories.
    public class AjoutLivreViewModel
    {
        [Display(Name = "Editeur")] // Spécifie le nom à afficher pour la liste déroulante des éditeurs.
        public List<SelectListItem> editeurs { get; set; } = new List<SelectListItem>(); // Déclare une propriété pour stocker les éléments de la liste déroulante des éditeurs.

        [Display(Name = "Categories")] // Spécifie le nom à afficher pour la liste déroulante des catégories.
        public List<SelectListItem> categories { get; set; } = new List<SelectListItem>(); // Déclare une propriété pour stocker les éléments de la liste déroulante des catégories.

        [Display(Name = "")] // Spécifie le nom à afficher pour le livre (non utilisé).
        public Livre livre { get; set; } = new Livre(); // Déclare une propriété pour stocker les données du livre.

    }
}
