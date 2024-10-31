
let params = new URLSearchParams(document.location.search);
let id = params.get("ID");
$(document).ready(function () {
    if (id) {
        getTagById()

    }
})
function addUpdateTag() {
    let tagObj = {
        "tag_id": id ? id : 0,
        "tag_name": $("#txtTag").val(),
        "slug": $("#txtSlug").val(),
        "description": $("#txtDescription").val(),
    }
    let apiUrl = id ? getApiUrl() + "Tag/UpdateTag" : getApiUrl() + "Tag/AddTag"
    $.ajax({
        url: apiUrl,
        type: "Post",
        contentType: "application/json",
        dataType: "JSON",
        data: JSON.stringify(tagObj),
        success: function (data) {
        
             if (id) {
                toastr.options = {
                    "onHidden": function () {
                        window.location.href = window.location.origin + "/home/viewtags"; // Redirect after toastr closes
                    }
                };
                toastr.success("Category updated successfully");
            } else {
                toastr.options = {
                    "onHidden": function () {
                        window.location.href =window.location.origin + "/home/viewtags"; // Redirect after toastr closes
                    }
                };
                 toastr.success("Tag added successfully");
            }
        },
        error: function (err) {
            console.log(err)
            toastr.error(err)
        }

    })
}

function getTagById() {
    $.ajax({
        url: getApiUrl() + "tag/GetAllTagsById/" + id,
        type: "Get",
        success: function (data) {
            $("#txtTag").val(data.tag_name)
            $("#txtSlug").val(data.slug)
            $("#txtDescription").val(data.description)
        },
        error: function (err) {
            getToasterOption()
            toastr.error("Something Went Wrong!");
        }
    })
}
