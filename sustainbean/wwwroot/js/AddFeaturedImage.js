
let params = new URLSearchParams(document.location.search);
let id = params.get("ID");
images =[]
$(document).ready(function () {
    if (id) {
        getImageById()

    }
})


$('#txtImage').on('change', function () {
    var files = $(this)[0].files;
    if (files.length > 0) {
        var base64Images = [];
        var reader = new FileReader();
        var counter = 0;
        reader.onload = function (e) {
            // Extract the Base64 string without the prefix and push it to the array
            var base64String = e.target.result.split(',')[1];
            var path = new Date().toISOString().replace(/\D/g, '') + "." + e.target.result.split(';')[0].split(':')[1].split('/')[1]
            console.log("Path : " + path)
            base64Images.push({ "img": base64String, "path": path });
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
            images = response
            console.log(response)
            displayPreview(response)
        },
        error: function (error) {
            // Handle the error response here
            console.error('Upload failed:', error);
        }
    });
}


function addUpdateImage() {
    let currentDate = new Date().toISOString();

    let imgObj = {
        "feature_id": id ? id : 0,
        "title": $("#txtTitle").val(),
        "alt_text": $("#txtAlt").val(),
        "caption": $("#txtCaption").val(),
        "description": $("#txtDescription").val(),
        "img_url": images[0],
    }
    console.log(imgObj)
    let apiUrl = id ? getApiUrl() + "Feature/UpdateFeature" : getApiUrl() + "Feature/AddFeature"
    $.ajax({
        url: apiUrl,
        type: "Post",
        contentType: "application/json",
        dataType: "JSON",
        data: JSON.stringify(imgObj),
        success: function (data) {
            console.log("image added successfully")
            window.location.href =window.location.origin + "/home/viewimages"
        },
        error: function (err) {
            console.log(err)
        }

    })
}
function displayPreview(images) {
    var previewContainer = $('#previewContainer ul');
    for (var i = 0; i < images.length; i++) {
        var imageUrl = images[i];
        var image = $('<img>').attr({
            'src': imageUrl,
            'class': 'image-preview',
            'height': '150',
            'width': '150'
        });
        // Create a delete button
        var deleteBtn = $('<span>').addClass('remove-image-btn').text('x');

        // Create a list item to hold the image and delete button
        var listItem = $('<li>').append(image).append(deleteBtn);

        // Use an IIFE to create a separate scope for each iteration
        (function (item) {
            // Add an event handler to the delete button to remove the image when clicked
            deleteBtn.on('click', function () {
                item.remove();
            });
        })(listItem);
        previewContainer.append(listItem);
    }
    // Enable sorting for images
    previewContainer.sortable();
}
function getImageById() {
    $.ajax({
        url: getApiUrl() + "Feature/" + id,
        type: "Get",
        success: function (data) {
            $("#txtTitle").val(data.title)
            $("#txtAlt").val(data.alt_text)
            $("#txtCaption").val(data.caption)
            $("#txtDescription").val(data.description)
            images = [data.img_url]
            displayPreview([data.img_url])

        },
        error: function (err) {
            getToasterOption()
            toastr.error("Something Went Wrong!");
        }
    })
}
