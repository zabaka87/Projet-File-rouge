using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace MagicBook.Models
{
    
        public class AjoutLivreViewModel
    {
        [Display(Name = "Editeur")]
        public List<SelectListItem> editeurs { get; set; } = new List<SelectListItem>();

        [Display(Name = "Categories")]
            public List<SelectListItem> categories { get; set; } = new List<SelectListItem>();


            [Display(Name = "")]
            public Livre livre { get; set; } = new Livre();



        }
    
}
