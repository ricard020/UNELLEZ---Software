document.querySelectorAll('.like-btn').forEach(button => {
    button.addEventListener('click', function (event) {
        event.stopPropagation(); // Evita que el evento se propague
        event.preventDefault();  // Evita cualquier comportamiento por defecto (como recargar la página)

        const postId = this.getAttribute('data-post-id');
        const userId = this.getAttribute('data-user-id');
        const likeCountElem = this.querySelector('.like-count');
        const likeTextElem = this.querySelector('.like-text');
        const buttonElem = this;

        // Validar datos
        if (!postId || !userId) {
            alert('Error: Datos incompletos.');
            return;
        }

        // Deshabilitar el botón temporalmente
        button.disabled = true;

        // Realizar la solicitud AJAX
        fetch('../controllers/like_action.php', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `post_id=${postId}&user_id=${userId}`
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    let currentCount = parseInt(likeCountElem.textContent);

                    if (data.action === 'added') {
                        likeCountElem.textContent = currentCount + 1;
                        buttonElem.classList.add('liked');
                        likeTextElem.textContent = '';
                    } else if (data.action === 'removed') {
                        likeCountElem.textContent = currentCount - 1;
                        buttonElem.classList.remove('liked');
                        likeTextElem.textContent = '';
                    }
                } else {
                    alert('Error: ' + (data.error || 'No se pudo procesar la solicitud.'));
                }
            })
            .catch(error => console.error('Error:', error))
            .finally(() => {
                button.disabled = false; // Rehabilitar el botón
            });
    });
});