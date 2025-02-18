// Función de JavaScript para manejar retweets
document.querySelectorAll('.retweet-btn').forEach(button => {
    button.addEventListener('click', function() {
        const postId = this.getAttribute('data-post-id');
        const userId = this.getAttribute('data-user-id');
        const retweetCountElem = this.querySelector('.retweet-count');
        const retweetTextElem = this.querySelector('.retweet-text');
        const buttonElem = this;

        // Realizar la solicitud AJAX para manejar retweet
        fetch('../controllers/retweet.php', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `post_id=${postId}&user_id=${userId}`
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                let currentCount = parseInt(retweetCountElem.textContent);

                if (data.action === 'added') {
                    retweetCountElem.textContent = currentCount + 1;
                    buttonElem.classList.add('retweeted');
                    retweetTextElem.textContent = '';
                } else if (data.action === 'removed') {
                    retweetCountElem.textContent = currentCount - 1;
                    buttonElem.classList.remove('retweeted');
                    retweetTextElem.textContent = '';
                }
            } else {
                alert('Error: ' + (data.error || 'No se pudo procesar la solicitud.'));
            }
        })
        .catch(error => console.error('Error:', error));
    });
});
