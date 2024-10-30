
let params = new URLSearchParams(document.location.search);
let id = params.get("ID");
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
        data: JSON.stringify(base64Images[0]),
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


function addUpdateImage() {
    let currentDate = new Date().toISOString();

    let imgObj = {
        "feature_id": id ? id : 0,
        "title": $("#txtTitle").val(),
        "alt_text": $("#txtAlt").val(),
        "caption": $("#txtCaption").val(),
        "description": $("#txtDescription").val(),

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
        },
        error: function (err) {
            console.log(err)
        }

    })
}

function getImageById() {
    $.ajax({
        url: getApiUrl() + "Feature/GetAllFeatureById/" + id,
        type: "Get",
        success: function (data) {
            $("#txtTitle").val(data.title)
            $("#txtAlt").val(data.alt_text)
            $("#txtCaption").val(data.caption)
            $("#txtDescription").val(data.description)

        },
        error: function (err) {
            getToasterOption()
            toastr.error("Something Went Wrong!");
        }
    })
}
