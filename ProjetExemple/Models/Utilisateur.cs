using System.ComponentModel.DataAnnotations;

namespace MagicBook.Models
{
    public class Utilisateur
    {
        [Display(Name = "IdUtilisateur")]
        public  int IdUtilisateur { get; set; }
        [Display(Name = "Administrateur")]
        public int Administrateur { get; set; }

        [Display(Name = "Nom")]
        public string NomUtilisateur { get; set; }
        [Display(Name = "Prenom")]
        public string PrenomUtilisateur { get; set; }

        [Display(Name = "Pseudo")]
        public string PseudoUtilisateur { get; set; }

       

        [Display(Name = "Email")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Veuillez entrer une adresse email valide.")]
        public string EmailUtilisateur { get; set; }
        [Display(Name = "Adresse1")]
        public string Adresse1 { get; set; }
        [Display(Name = "Adresse2")]
        public string? Adresse2 { get; set; }

        [Display(Name = "CodePostal")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Veuillez entrer un code postal valide.")]
        public string CodePostal { get; set; }

        [Display(Name = "Ville")]
        public string Ville { get; set; }

        [Display(Name = "Nombre jetons")]
        public int NBJetons { get; set; }

        [Display(Name = "Date d'inscription")]
        public DateTime DateInscription { get; set; }


        [Display(Name = "Mot De Passe")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères, une lettre majuscule, une lettre minuscule, un chiffre et un caractère spécial.")]
        public string Password { get; set; }

        public bool KeepLoggedIn { get; set; }



    }

}
