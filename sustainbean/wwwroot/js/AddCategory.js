
let params = new URLSearchParams(document.location.search);
let id = params.get("ID");
$(document).ready(function () {
    if (id) {
        getCategoryById()

    }
})
function addUpdateCategory() {
    let currentDate = new Date().toISOString();

    let categoryObj = {
        "category_id": id ? id : 0,
        "category": $("#txtCategory").val(),
        "slug": $("#txtSlug").val(),
        "parent_category": $("#drpParentCat").val() ? $("#drpParentCat").val() : "0",
        "description": $("#txtDescription").val(),
        "is_active": true,
        "created_on": currentDate,
        "created_by": "0",
        "updated_on": currentDate,
        "updated_by": "0"
    }
    console.log(categoryObj)
    let apiUrl = id ? getApiUrl() + "Category/UpdateCategory" : getApiUrl() + "Category/AddCategory"
    $.ajax({
        url: apiUrl,
        type: "Post",
        contentType: "application/json",
        dataType: "JSON",
        data: JSON.stringify(categoryObj),
        success: function (data) {
            console.log("category added successfully")
        },
        error: function (err) {
            console.log(err)
        }

    })
}

function getCategoryById() {
    $.ajax({
        url: getApiUrl() + "Category/GetAllCategoryById/" + id,
        type: "Get",
        success: function (data) {
            $("#txtCategory").val(data.category)
            $("#txtSlug").val(data.slug)
            $("#drpParentCat").val(data.parent_category)
            $("#inputCategoryName").val(data.category_name)
            $("#txtDescription").val(data.description)

        },
        error: function (err) {
            getToasterOption()
            toastr.error("Something Went Wrong!");
        }
    })
}
