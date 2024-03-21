using System.ComponentModel.DataAnnotations;

namespace ProjetExemple.Models
{
    public class Utilisateur
    {
        [Display(Name = "IdUtilisateur")]
        public  int IdUtilisateur { get; set; }
        [Display(Name = "Administrateur")]
        public int Administrateur { get; set; }
        [Display(Name = "Nom")]
        public string? NomUtilisateur { get; set; }
        [Display(Name = "Prenom")]
        public string? PrenomUtilisateur { get; set; }

        [Display(Name = "Pseudo")]
        public string PseudoUtilisateur { get; set; }
        [Display(Name = "Email")]
        public string EmailUtilisateur { get; set; }
        [Display(Name = "Adresse1")]
        public string Adresse1 { get; set; }
        [Display(Name = "Adresse2")]
        public string? Adresse2 { get; set; }

        [Display(Name = "CodePostal")]
        public string CodePostal { get; set; }
        [Display(Name = "Ville")]
        public string Ville { get; set; }
        [Display(Name = "Nombre jetons")]
        public int NBJetons { get; set; }
        [Display(Name = "Date d'inscription")]
        public DateTime DateInscription { get; set; }
    }
}
