
let params = new URLSearchParams(document.location.search);
let id = params.get("ID");
$(document).ready(function () {
    if (id) {
        getImageById()

    }
})
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
        url: getApiUrl() + "Feature//GetAllFeatureById/" + id,
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
