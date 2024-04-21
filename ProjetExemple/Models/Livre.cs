using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Globalization;

namespace MagicBook.Models
{
    public class Livre
    {


        [Required(ErrorMessage = "Ce champ est Obligatoire.")]
        [StringLength(14, MinimumLength = 5, ErrorMessage = "un ISBN doit être 13 caractères.")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Ce champ est Obligatoire.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "La longueur doit être superieur a 10 caractéres.")]
        [Display(Name = "Titre")]
        public string TitreLivre { get; set; }

        [Display(Name = "Resumer")]
        public string? ResumeLivre { get; set; }


        //[Required(ErrorMessage = "Ce champ est Obligatoire.")]
        //[StringLength(13, MinimumLength = 13, ErrorMessage = "Date de publication Obligatoire.")]
        [Display(Name = "Date Publication")]
        public DateTime? DatePublicationLivre { get; set; }

        //[Required(ErrorMessage = "Ce champ est Obligatoire.")]
        //[StringLength(13, MinimumLength = 13, ErrorMessage = "Entrer Le Nom De L'editeur.")]
        [Display(Name = "Editeur")]
        public int IdEditeur { get; set; }

        //[Required(ErrorMessage = "Ce champ est Obligatoire.")]
        //[StringLength(13, MinimumLength = 13, ErrorMessage = "veuiller choisire le genre.")]
        [Display(Name = "Genre")]
        public int IdCategorie { get; set; }

        [Display(Name = "Image")]
        public string? image { get; set; }


       
    }
}
