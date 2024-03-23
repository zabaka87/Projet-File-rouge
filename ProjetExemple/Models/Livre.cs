using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace MagicBook.Models
{
    public class Livre
    {
        [Required(ErrorMessage = "Ce champ est Obligatoire.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "un ISBN doit être 13 caractères.")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Ce champ est Obligatoire.")]
        [StringLength(28, MinimumLength = 10, ErrorMessage = "La longueur doit être superieur a 10 caractéres.")]
        [Display(Name = "Titre")]
        public string TitreLivre { get; set; }

        [Display(Name = "Resumer")]
        public string? ResumeLivre { get; set; }


        [Required(ErrorMessage = "Ce champ est Obligatoire.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Date de publication Obligatoire.")]
        [Display(Name = "Date Publication")]
        public DateOnly? DatePublicationLivre { get; set; }

        [Required(ErrorMessage = "Ce champ est Obligatoire.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Entrer Le Nom De L'editeur.")]
        [Display(Name = "Editeur")]
        public int IdEditeur { get; set; }

        [Required(ErrorMessage = "Ce champ est Obligatoire.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "veuiller choisire le genre.")]
        [Display(Name = "Genre")]
        public int IdCategorie { get; set; }

    }
}
