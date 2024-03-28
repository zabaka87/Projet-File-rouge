using System.ComponentModel.DataAnnotations;

namespace MagicBook.Models
{
    public class genres
    {
        [Display(Name = "IdCategorie")]
        public int IdCategorie { get; set; }
        [Display(Name = "Genre")]
        public string? NomCategorie { get; set; }

    }
}
