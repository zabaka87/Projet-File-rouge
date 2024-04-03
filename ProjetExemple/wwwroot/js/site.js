document.getElementById('next').onclick = function () {
    let lists = document.querySelectorAll('.item');
    document.getElementById('slide').appendChild(lists[0]);
}
document.getElementById('prev').onclick = function () {
    let lists = document.querySelectorAll('.item');
    document.getElementById('slide').prepend(lists[lists.length - 1]);
}
document.getElementById('contact-form').addEventListener('submit', function (e) {
    e.preventDefault();

    let name = document.getElementById('name').value;
    let email = document.getElementById('email').value;
    let message = document.getElementById('message').value;

    // Envoi des données à un serveur ou traitement des données

    document.getElementById('response').innerHTML = `<p>Merci ${name}! Votre message a été envoyé avec succès.</p>`;
    document.getElementById('contact-form').reset();
});
