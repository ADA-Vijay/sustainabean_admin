$(document).ready(function () {
    $('#drpTag').select2()
})


// Initialize TinyMCE
tinymce.init({
    selector: "#editor",
    plugins: [
        "anchor", "autolink", "charmap", "codesample", "emoticons", "image", "link", "lists", "media", "searchreplace", "table", "visualblocks", "wordcount"
    ],
    content_css: './textEditor.css?myParam=myValue&bogus=' + new Date().getTime(),
    toolbar: "undo redo | bold italic underline strikethrough | link image media table | removeformat",
    style_formats: [
        { title: 'Red Text', inline: 'span', styles: { color: '#ff0000' } },
        { title: 'Bold text', inline: 'strong' },
        { title: 'Italic text', inline: 'em' }
    ],
    content_css_cors: false, 
    content_style: `
    body {
        font - family: Helvetica, Arial, sans-serif;
    font-size: 14px;
    padding: 1rem 7rem !important;
    display: flex;
    flex-direction: column;
    justify-content: center; 
          }
    img {
        display: block;
    margin-left: auto;
    margin-right: auto; 
          }
    `, extended_valid_elements: 'p[style],span[style]',
    templates: [
        {
            title: 'Article Block',
            description: 'Add an article block with image, title, and description',
            content: `
    <div class="card">
        <img src="https://via.placeholder.com/300x200" class="card-img-top" alt="Article Image">
            <div class="card-body">
                <h5 class="card-title">Article Title</h5>
                <p class="card-text">By Author Name</p>
                <p class="card-text">Short description or excerpt of the article goes here.</p>
            </div>
    </div>
    `
        }
    ]
});


$("#print-html").click(function () {
    $("#container").html("")
    var htmlContent = tinymce.activeEditor.getContent();
    console.log(htmlContent);
    $("#container").html(htmlContent)
});
// Initialize TinyMCE
tinymce.init({
    selector: "#editor",
    plugins: [
        "anchor", "autolink", "charmap", "codesample", "emoticons", "image", "link", "lists", "media", "searchreplace", "table", "visualblocks", "wordcount"
    ],
    content_css: './textEditor.css?myParam=myValue&bogus=' + new Date().getTime(),
    toolbar: "undo redo | bold italic underline strikethrough | link image media table | removeformat",
    style_formats: [
        { title: 'Red Text', inline: 'span', styles: { color: '#ff0000' } },
        { title: 'Bold text', inline: 'strong' },
        { title: 'Italic text', inline: 'em' }
    ],
    content_css_cors: false,
    content_style: `
    body {
        font - family: Helvetica, Arial, sans-serif;
    font-size: 14px;
    padding: 1rem 7rem !important;
    display: flex;
    flex-direction: column;
    justify-content: center; 
          }
    img {
        display: block;
    margin-left: auto;
    margin-right: auto; 
          }
    `, extended_valid_elements: 'p[style],span[style]',
    templates: [
        {
            title: 'Article Block',
            description: 'Add an article block with image, title, and description',
            content: `
    <div class="card">
        <img src="https://via.placeholder.com/300x200" class="card-img-top" alt="Article Image">
            <div class="card-body">
                <h5 class="card-title">Article Title</h5>
                <p class="card-text">By Author Name</p>
                <p class="card-text">Short description or excerpt of the article goes here.</p>
            </div>
    </div>
    `
        }
    ]
});

$("#print-html").click(function () {
    $("#container").html("")
    var htmlContent = tinymce.activeEditor.getContent();
    console.log(htmlContent);
    $("#container").html(htmlContent)
});
// Initialize TinyMCE
tinymce.init({
    selector: "#editor",
    plugins: [
        "anchor", "autolink", "charmap", "codesample", "emoticons", "image", "link", "lists", "media", "searchreplace", "table", "visualblocks", "wordcount"
    ],
    content_css: './textEditor.css?myParam=myValue&bogus=' + new Date().getTime(),
    toolbar: "undo redo | bold italic underline strikethrough | link image media table | removeformat",
    style_formats: [
        { title: 'Red Text', inline: 'span', styles: { color: '#ff0000' } },
        { title: 'Bold text', inline: 'strong' },
        { title: 'Italic text', inline: 'em' }
    ],
    content_css_cors: false,
    content_style: `
    body {
        font - family: Helvetica, Arial, sans-serif;
    font-size: 14px;
    padding: 1rem 7rem !important;
    display: flex;
    flex-direction: column;
    justify-content: center; 
          }
    img {
        display: block;
    margin-left: auto;
    margin-right: auto; 
          }
    `, extended_valid_elements: 'p[style],span[style]',
    templates: [
        {
            title: 'Article Block',
            description: 'Add an article block with image, title, and description',
            content: `
    <div class="card">
        <img src="https://via.placeholder.com/300x200" class="card-img-top" alt="Article Image">
            <div class="card-body">
                <h5 class="card-title">Article Title</h5>
                <p class="card-text">By Author Name</p>
                <p class="card-text">Short description or excerpt of the article goes here.</p>
            </div>
    </div>
    `
        }
    ]
});

$("#print-html").click(function () {
    $("#container").html("")
    var htmlContent = tinymce.activeEditor.getContent();
    console.log(htmlContent);
    $("#container").html(htmlContent)
});
// Initialize TinyMCE
tinymce.init({
    selector: "#editor",
    plugins: [
        "anchor", "autolink", "charmap", "codesample", "emoticons", "image", "link", "lists", "media", "searchreplace", "table", "visualblocks", "wordcount"
    ],
    content_css: './textEditor.css?myParam=myValue&bogus=' + new Date().getTime(),
    toolbar: "undo redo | bold italic underline strikethrough | link image media table | removeformat",
    style_formats: [
        { title: 'Red Text', inline: 'span', styles: { color: '#ff0000' } },
        { title: 'Bold text', inline: 'strong' },
        { title: 'Italic text', inline: 'em' }
    ],
    content_css_cors: false,
    content_style: `
    body {
        font - family: Helvetica, Arial, sans-serif;
    font-size: 14px;
    padding: 1rem 7rem !important;
    display: flex;
    flex-direction: column;
    justify-content: center; 
          }
    img {
        display: block;
    margin-left: auto;
    margin-right: auto; 
          }
    `, extended_valid_elements: 'p[style],span[style]',
    templates: [
        {
            title: 'Article Block',
            description: 'Add an article block with image, title, and description',
            content: `
    <div class="card">
        <img src="https://via.placeholder.com/300x200" class="card-img-top" alt="Article Image">
            <div class="card-body">
                <h5 class="card-title">Article Title</h5>
                <p class="card-text">By Author Name</p>
                <p class="card-text">Short description or excerpt of the article goes here.</p>
            </div>
    </div>
    `
        }
    ]
});

$("#print-html").click(function () {
    $("#container").html("")
    var htmlContent = tinymce.activeEditor.getContent();
    console.log(htmlContent);
    $("#container").html(htmlContent)
});
// Initialize TinyMCE
tinymce.init({
    selector: "#editor",
    plugins: [
        "anchor", "autolink", "charmap", "codesample", "emoticons", "image", "link", "lists", "media", "searchreplace", "table", "visualblocks", "wordcount"
    ],
    toolbar: "undo redo | bold italic underline strikethrough | link image media table | removeformat",
    custom_elements: "emstart,emend",
    // File picker callback to handle image uploads
    file_picker_callback: function (callback, value, meta) {
        if (meta.filetype === 'image') {
            var input = document.createElement('input');
            input.setAttribute('type', 'file');
            input.setAttribute('accept', 'image/*');

            input.onchange = function () {
                var file = this.files[0];
                var formData = new FormData();
                formData.append('file', file);

                // AJAX upload to your server
                $.ajax({
                    url: '/upload-endpoint',  // Replace with your server-side upload URL
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        // Assuming the server returns the image URL
                        callback(response.imageUrl, { alt: file.name });
                    },
                    error: function (xhr, status, error) {
                        console.error("Image upload failed: " + error);
                    }
                });
            };

            input.click();
        }
    },

    // Custom image upload handler
    images_upload_handler: function (blobInfo, success, failure) {
        var formData = new FormData();
        formData.append('file', blobInfo.blob(), blobInfo.filename());

        // AJAX upload to your server
        $.ajax({
            url: '/upload-endpoint',  // Replace this with your server URL
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                // Assuming your server returns the URL of the uploaded image
                success(response.imageUrl);
            },
            error: function (xhr, status, error) {
                failure('Image upload failed: ' + error);
            }
        });
    },

    content_css: './textEditor.css?myParam=myValue&bogus=' + new Date().getTime(),
    content_style: `
        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 14px;
        }
        img {
            display: block;
            margin-left: auto;
            margin-right: auto;
        }
    `,

    extended_valid_elements: 'p[style],span[style]',
});




$("#print-html").click(function () {
    $("#container").html("")
    var htmlContent = tinymce.activeEditor.getContent();
    console.log(htmlContent);
    $("#container").html(htmlContent)
});


$('#imageGroup').on('click', function (event) {
    event.preventDefault(); 
    $('#featuredImageModal').modal('show'); 
});


let selectedImage = null;
$('.selectable-image').on('click', function () {
    $('.selectable-image').removeClass('selected');
    $(this).addClass('selected');
    selectedImage = $(this).attr('src');
});

$('#selectImageBtn').on('click', function () {
    if (selectedImage) {
        let imagePreview = $('<img>').attr('src', selectedImage).addClass('img-thumbnail');
        $('#divFeaturedImage').html(imagePreview);
        $('#imageModal').modal('hide');
    } else {
        alert('Please select an image');
    }
});
