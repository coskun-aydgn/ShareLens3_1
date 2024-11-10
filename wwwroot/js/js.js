// Display a preview of the uploaded image
function previewImage(event) {
    const reader = new FileReader();
    reader.onload = function() {
        const output = document.querySelector('.detail-page-image');
        output.src = reader.result; // Change the image source to the new uploaded image
    };
    reader.readAsDataURL(event.target.files[0]);
}
