using System.ComponentModel.DataAnnotations;

namespace MagicBook.Models
{
    public class Editeur
    {
        [Display(Name = "IdEditeur")]
        public int IdEditeur { get; set; }
        [Display(Name ="Editeur")]
        public string? NomEditeur{ get; set; }
    }
}
