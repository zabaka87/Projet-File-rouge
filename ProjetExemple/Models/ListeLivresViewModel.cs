namespace MagicBook.Models // Définit le namespace MagicBook.Models pour les modèles de l'application.
{
    // Définit la classe ListeLivresViewModel pour représenter une liste de livres dans la vue.
    public class ListeLivresViewModel
    {
        // Déclare une propriété pour stocker une liste de LivreViewModel.
        // Cette liste représente les détails des livres à afficher dans la vue.
        public List<LivreViewModel> livres { get; set; } = new List<LivreViewModel>();
    }
}
