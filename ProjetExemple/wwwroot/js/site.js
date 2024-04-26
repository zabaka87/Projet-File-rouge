// Récupération de l'élément HTML avec l'ID "bouton_recherche"
let button_recherche = document.getElementById("bouton_recherche");

// Ajout d'un écouteur d'événement sur le bouton de recherche, qui se déclenche lors d'un clic
button_recherche.addEventListener("click", () => {
    // Récupération de la valeur de l'élément HTML avec l'ID "champ_recherche"
    let champ_recherche = document.getElementById("champ_recherche").value;

    // Envoi d'une requête HTTP GET à l'API locale avec la valeur du champ de recherche en paramètre
    fetch("http://localhost:5089/Accueil/rechercher?ThemeRecherche=" + champ_recherche).then(
        res => {
            // Conversion de la réponse en format JSON
            return res.json();
        }
    )
        .then(json => {
            // Suppression des livres déjà présents dans la section des livres
            let livres = document.querySelectorAll("div.cadre_info_livre,div.livre-main ");
            livres.forEach(l => l.remove());

            // Vérification si aucun livre n'a été trouvé
            if (json.length === 0) {
                // Création d'un nouvel élément HTML pour afficher un message d'erreur
                let divlivre = document.createElement("div");
                divlivre.className = "cadre_info_livre";

                let paragraphe = document.createElement("p");
                paragraphe.textContent = "Aucun livre trouvé.Voulez vous ajouter le livre?";

                // Création d'un lien pour ajouter un livre
                let lienPageAjout = document.createElement("a");
                let cheminPageDAjout = "/Livre/Ajouter/";
                lienPageAjout.textContent = "Ajouter un livre";
                lienPageAjout.href = cheminPageDAjout;
                paragraphe.appendChild(lienPageAjout);

                // Ajout du message d'erreur et du lien à la div créée précédemment
                divlivre.appendChild(paragraphe);
                let section_livres = document.getElementById("Presentation_livres");
                section_livres.appendChild(divlivre);
            } else {
                // Boucle sur chaque livre trouvé dans la réponse JSON
                for (var i = 0; i < json.length; i++) {
                    // Création d'un nouvel élément HTML pour afficher les informations du livre
                    let section_livres = document.getElementById("Presentation_livres");
                    section_livres.appendChild(CreateCadreLivre(json[i])); // Traiter chaque livre individuellement
                }
            }
        });
});

// Définition de la fonction CreateCadreLivre() qui prend un objet 'livre' en paramètre
function CreateCadreLivre(livre) {

    // Création d'un nouvel élément HTML pour la div du livre
    let divlivre = document.createElement("div");
    divlivre.className = "cadre_info_livre";
    divlivre.id = "info_livre";

    // Création d'une div pour l'image de couverture du livre
    let divImage = document.createElement("div");
    divImage.className = "image_couv";

    // Récupération de l'ISBN du livre
    let isbn = livre.isbn;

    // Création de l'URL pour la page de détail du livre
    let url = "/Livre/Detail/" + isbn;

    // Création d'un lien HTML pour rediriger vers la page de détail du livre
    let lienDetailLivre = document.createElement("a");
    lienDetailLivre.href = url;

    // Création d'un élément HTML pour l'image de couverture du livre
    let image = document.createElement("img");
    image.setAttribute("src", livre.photo);
    image.className = "Photo_couv";

    // Ajout de l'image de couverture au lien HTML
    lienDetailLivre.appendChild(image);

    // Ajout du lien HTML à la div de l'image de couverture
    divImage.appendChild(lienDetailLivre);

    // Ajout de la div de l'image de couverture à la div du livre
    divlivre.appendChild(divImage);

    // Création d'une div pour les informations du livre
    let divInfos = document.createElement("div");
    divInfos.className = "infos_livre";

    // Création d'un paragraphe HTML pour le titre du livre
    let paragrapheTitre = document.createElement("p");
    paragrapheTitre.textContent = livre.titreLivre;
    paragrapheTitre.className = "titre";

    // Ajout du titre du livre à la div des informations
    divInfos.appendChild(paragrapheTitre);

    // Création d'un paragraphe HTML pour l'ISBN du livre
    let paragrapheISBN = document.createElement("p");
    paragrapheISBN.textContent = livre.isbn;
    paragrapheISBN.className = "ISBN";

    // Ajout de l'ISBN du livre à la div des informations
    divInfos.appendChild(paragrapheISBN);

    // Création d'un paragraphe HTML pour les auteurs du livre
    let paragrapheAuteurs = document.createElement("p");

    // Boucle sur chaque auteur du livre
    livre.auteurs.forEach(auteur => {
        // Création d'une liste HTML pour chaque auteur
        let listeAuteur = document.createElement("ul");
        let elementAuteur = document.createElement("li");
        elementAuteur.textContent = auteur.nomAuteur + " " + auteur.prenomAuteur;
        listeAuteur.appendChild(elementAuteur);
        paragrapheAuteurs.appendChild(listeAuteur);
    });

    // Ajout des auteurs du livre à la div des informations
    divInfos.appendChild(paragrapheAuteurs);

    // Création d'un paragraphe HTML pour l'éditeur du livre
    let paragrapheEditeur = document.createElement("p");
    paragrapheEditeur.textContent = livre.nomEditeur;
    paragrapheEditeur.className = "editeur";

    // Ajout de l'éditeur du livre à la div des informations
    divInfos.appendChild(paragrapheEditeur);

    // Création d'un paragraphe HTML pour la catégorie du livre
    let paragrapheCategorie = document.createElement("p");
    paragrapheCategorie.textContent = livre.nomCategorie;
    paragrapheCategorie.className = "categorie";

    // Ajout de la catégorie du livre à la div des informations
    divInfos.appendChild(paragrapheCategorie);

    // Création d'un paragraphe HTML pour la date de publication du livre
    let paragrapheDatePublication = document.createElement("p");
    paragrapheDatePublication.textContent = livre.datePublicationLivre;
    paragrapheDatePublication.className = "date_de_publication";

    // Ajout de la date de publication du livre à la div des informations
    divInfos.appendChild(paragrapheDatePublication);

    // Ajout de la div des informations à la div du livre
    divlivre.appendChild(divInfos);

    // Retour de la div du livre
    return divlivre;
}

