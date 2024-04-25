using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Globalization;

namespace MagicBook.Models
{
    public class Livre
    {
        // Propriété ISBN du livre
        [Required(ErrorMessage = "Ce champ est Obligatoire.")] // Annotation pour définir que ce champ est requis
        [StringLength(14, MinimumLength = 5, ErrorMessage = "un ISBN doit être 13 caractères.")] // Annotation pour définir la longueur de la chaîne
        [Display(Name = "ISBN")] // Annotation pour définir le nom à afficher dans l'interface utilisateur
        public string ISBN { get; set; }

        // Propriété TitreLivre du livre
        [Required(ErrorMessage = "Ce champ est Obligatoire.")] // Annotation pour définir que ce champ est requis
        [StringLength(50, MinimumLength = 5, ErrorMessage = "La longueur doit être superieur a 10 caractéres.")] // Annotation pour définir la longueur de la chaîne
        [Display(Name = "Titre")] // Annotation pour définir le nom à afficher dans l'interface utilisateur
        public string TitreLivre { get; set; }

        // Propriété ResumeLivre du livre
        [Display(Name = "Resumer")] // Annotation pour définir le nom à afficher dans l'interface utilisateur
        public string? ResumeLivre { get; set; } // Propriété pouvant être null

        // Propriété DatePublicationLivre du livre
        [Display(Name = "Date Publication")] // Annotation pour définir le nom à afficher dans l'interface utilisateur
        public DateTime? DatePublicationLivre { get; set; } // Propriété pouvant être null

        // Propriété IdEditeur du livre
        [Display(Name = "Editeur")] // Annotation pour définir le nom à afficher dans l'interface utilisateur
        public int IdEditeur { get; set; }

        // Propriété IdCategorie du livre
        [Display(Name = "Genre")] // Annotation pour définir le nom à afficher dans l'interface utilisateur
        public int IdCategorie { get; set; }

        // Propriété image du livre
        [Display(Name = "Image")] // Annotation pour définir le nom à afficher dans l'interface utilisateur
        public string? image { get; set; } // Propriété pouvant être null
    }
}

