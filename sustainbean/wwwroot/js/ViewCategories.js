$(document).ready(function () {
    getActiveLink('manageCategories');
    getAllCategory();
});

function getAllCategory() {
    $("#tbl-category").DataTable({
        paging: true,
        processing: true,
        serverSide: true,
        pageLength: 10,
        ajax: {
            url: getApiUrl() + "Category/GetAllCategory",
            type: "POST",
            dataSrc: "data",
            error: function (xhr, textStatus, errorThrown) {
                toastr.error("Something Went Wrong!");
            }
        },
        columns: [
            { data: "category" },
            { data: "slug" },
            { data: "parent_category" },
            { data: "description" },
            {
                data: null,
                render: function (data, type, row, meta) {
                    return `<button class="btn btn-sm btn-secondary btnEdit" onclick="redirect('${row.category_id}')">Edit</button>`;
                }
            }
        ],
        language: {
            lengthMenu: "Show _MENU_",
            paginate: {
                next: ">",
                previous: "<"
            }
        }
    });

}

function redirect(id) {
    window.location.href = window.location.origin + "/Home/AddCategory/?ID=" + id;
}
