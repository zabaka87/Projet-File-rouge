using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace MagicBook.Models
{
    public class Livre
    {
       
            [Display(Name = "ISBN")]
            public string ISBN { get; set; }
            [Display(Name = "Titre")]
            public string TitreLivre { get; set; }
            [Display(Name = "Resumer")]
            public string? ResumeLivre { get; set; }
            [Display(Name = "Date Publication")]
            public DateTime? DatePublicationLivre { get; set; }
            [Display(Name = "Editeur")]
            public int IdEditeur { get; set; }
            [Display(Name = "Categorie")]
            public int IdCategorie { get; set; }         
        
    }
}
