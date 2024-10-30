
let params = new URLSearchParams(document.location.search);
let id = params.get("ID");
var images = [];

$(document).ready(function () {
    if (id) {
        getBlogById()

    }
    getAllTag()
    getAllCategories()
    getAllImage()
});
$('#txtImage').on('change', function () {
    var files = $(this)[0].files;
    if (files.length > 0) {
        var base64Images = [];
        var reader = new FileReader();
        var counter = 0;
        reader.onload = function (e) {
            // Extract the Base64 string without the prefix and push it to the array
            var base64String = e.target.result.split(',')[1];
            var path =   new Date().toISOString().replace(/\D/g, '') + "." + e.target.result.split(';')[0].split(':')[1].split('/')[1]
            console.log("Path : " + path)
            base64Images.push({ img: base64String, path: path });
            if (counter < files.length - 1) {
                counter++;
                reader.readAsDataURL(files[counter]);
            } else {
                // All images are converted to Base64, now call the function to send the array
                uploadImagesToCdn(base64Images);
            }
        };

        // Start the initial Base64 conversion
        reader.readAsDataURL(files[counter]);
    }
});

function uploadImagesToCdn(base64Images) {
    // Make the AJAX call
    var apiUrl = getApiUrl() + 'CDN';
    $.ajax({
        url: apiUrl,
        type: 'POST',
        data: JSON.stringify(base64Images),
        contentType: 'application/json',
        success: function (response) {
       
            displayPreview(response.respObj)
        },
        error: function (error) {
            // Handle the error response here
            console.error('Upload failed:', error);
        }
    });
}

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
        font-family: Helvetica, Arial, sans-serif;
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
    `,
    extended_valid_elements: 'p[style],span[style]',
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
    ],
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
});

function getAllTag() {
   
        $.ajax({
            url: getApiUrl() + "Tag/GetAllTagsList" ,
            type: "Get",
            success: function (data) {
                var options = ""
                $("#drpTag").html("")
                data.forEach(e => {
                    options += `<option value="${e.tag_id}">${e.tag_name}</option>`
                })
                $("#drpTag").append(options)
                $("#drpTag").prop("disabled", false).trigger("change")
            },
            error: function (err) {
                getToasterOption()
                toastr.error("Something Went Wrong!");
            }
        })
    

}
function getAllCategories() {

    $.ajax({
        url: getApiUrl() + "Category/GetAllCategoryList",
        type: "Get",
        success: function (data) {
            var options = ""
            $("#drpCategory").html("")
            data.forEach(e => {
                options += `<option value="${e.category_id}">${e.category}</option>`
            })
            $("#drpCategory").append(options)
            $("#drpCategory").prop("disabled", false).trigger("change")
        },
        error: function (err) {
            getToasterOption()
            toastr.error("Something Went Wrong!");
        }
    })


}

$("#print-html").click(function () {
    $("#container").html("");
    var htmlContent = tinymce.activeEditor.getContent();
    console.log(htmlContent);
    $("#container").html(htmlContent);
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

function getAllImage() {
    $.ajax({
        url: getApiUrl() +"Feature/GetAllFeaturesList",
        type: "get",
        contentType: "application/json",
        success: function (data) {
            var options = ""
            $("#drpImage").html("")
            data.forEach(e => {
                options += `<option value="${e.image_url}">${e.title}</option>`
            })
            $("#drpImage").append(options)
            $("#drpImage").prop("disabled", false).trigger("change")
        },
        error: function (err) {
            console.log(err)
        }

    })


}


function addUpdateBlogs() {
    let blogObj = {
        "blog_id": id ? id : 0,
        "category_id": 0,
        "tag_id": $("#drpTag").val(),
        "slug": $("#txtSlug").val(),
        "auther": $("#txtAuthor").val(),
        "img_url": $("##drpImage").val(),
        "seo_title": $("#txtSeoTitle").val(),
        "seo_key_word": $("#txtSeoKey").val(),
        "description": $("#txtSeoDescription").val(),
        "html": tinymce.activeEditor.getContent(), 
    }
    console.log(blogObj)
    let apiUrl = id ? "Blog/UpdateBlog" : "Blog/AddBlog"

    $.ajax({
        url: getApiUrl() + apiUrl,
        type: "Post",
        contentType: "application/json",
        dataType: "JSON",
        data: JSON.stringify(blogObj),
        success: function (data) {
            console.log("blog added successfully")
        },
        error: function (err) {
            console.log(err)
        }

    })
}
