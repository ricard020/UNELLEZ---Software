document.querySelectorAll('.comment-btn2').forEach(button => {
    button.addEventListener('click', function () {
        const postId = this.getAttribute('data-post-id');
        const userId = this.getAttribute('data-user-id');

        // Limpiar el campo de texto del comentario antes de mostrar la modal
        document.getElementById('commentText').value = "";

        // Obtener los detalles del proyecto (título y descripción)
        fetch(`../controllers/obtener_detalles_proyecto.php?post_id=${postId}`)
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    // Rellenar la modal con los datos del proyecto 
                    document.getElementById('projectTitle').textContent = data.project.titulo;
                    document.getElementById('projectDescription').textContent = data.project.descripcion;

                    // Mostrar la modal
                    const commentModal = new bootstrap.Modal(document.getElementById('commentModal'));
                    commentModal.show();

                    // Manejar el envío del comentario
                    const submitCommentButton = document.getElementById('submitComment');

                    // Elimina cualquier evento previo para evitar duplicados
                    submitCommentButton.replaceWith(submitCommentButton.cloneNode(true));
                    document.getElementById('submitComment').addEventListener('click', function () {
                        const commentText = document.getElementById('commentText').value;
                        if (commentText) {
                            fetch('../controllers/comentario.php', {
                                method: 'POST',
                                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                                body: `post_id=${postId}&user_id=${userId}&comment_text=${encodeURIComponent(commentText)}`
                            })
                                .then(response => response.json())
                                .then(data => {
                                    if (data.success) {
                                        // Forzar recarga de la página actual
                                        window.location = `${window.location.origin}/social_network/controllers/publicacion_detalle.php?post_id=${postId}`;
                                    } else {
                                        alert('Error al agregar el comentario: ' + (data.error || 'No se pudo procesar la solicitud.'));
                                    }
                                })
                                .catch(error => console.error('Error:', error));
                        } else {
                            alert('Por favor, ingresa un comentario.');
                        }
                    });
                } else {
                    alert('No se pudieron obtener los detalles del proyecto.');
                }
            })
            .catch(error => console.error('Error:', error));
    });
});
