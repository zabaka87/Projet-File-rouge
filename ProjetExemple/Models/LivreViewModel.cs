using System.ComponentModel.DataAnnotations;

namespace MagicBook.Models
{
    public class LivreViewModel
    {
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }
        [Display(Name = "Titre")]
        public string? TitreLivre { get; set; }
        [Display(Name = "Resumer")]
        public string? ResumeLivre { get; set; }
        [Display(Name = "Date Publication")]
        public DateTime? DatePublicationLivre { get; set; }
        [Display(Name = "Nom Editeur")]
        public required string NomEditeur { get; set; }
        [Display(Name = "Genre")]
        public required string NomCategorie { get; set; }
        [Display(Name = "Image")]
        public string Image { get; set; }

        public List<CommentaireViewModel>  Commentaire { get; set; } = new List<CommentaireViewModel>();

     

    }
  
}

