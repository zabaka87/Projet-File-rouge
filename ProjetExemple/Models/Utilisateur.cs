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


        [Display(Name = "Mot De Pass")]
        public int Password { get; set; }

        public Utilisateur(int idUtilisateur, int administrateur, string? nomUtilisateur, string? prenomUtilisateur, string pseudoUtilisateur, string emailUtilisateur, string adresse1, string? adresse2, string codePostal, string ville, int nbJetons, DateTime dateInscription)
        {
            IdUtilisateur = idUtilisateur;
            Administrateur = administrateur;
            NomUtilisateur = nomUtilisateur;
            PrenomUtilisateur = prenomUtilisateur;
            PseudoUtilisateur = pseudoUtilisateur;
            EmailUtilisateur = emailUtilisateur;
            Adresse1 = adresse1;
            Adresse2 = adresse2;
            CodePostal = codePostal;
            Ville = ville;
            NBJetons = nbJetons;
            DateInscription = dateInscription;
        }
    }

}
