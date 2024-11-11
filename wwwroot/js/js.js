// Display a preview of the uploaded image
function previewImage(event) {
    const reader = new FileReader();
    reader.onload = function() {
        const output = document.querySelector('.detail-page-image');
        output.src = reader.result; // Change the image source to the new uploaded image
    };
    reader.readAsDataURL(event.target.files[0]);
}

function addLike(postId) {
    fetch(`/Post/AddLike?postId=${postId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.newLikeCount !== undefined) {
            // Başarılıysa, LikeCount'u güncelle
            document.querySelector('.post-likes').textContent = `${data.newLikeCount} likes`;
        } else if (data.error) {
            console.error(data.error);
        }
    })
    .catch(error => {
        console.error('Like işlemi başarısız:', error);
    });
}