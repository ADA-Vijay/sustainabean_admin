$(document).ready(function () {
    getActiveLink('manageImages')
    getAllImage()
})



function getAllImage() {
    $("#tbl-image").DataTable({
        paging: true,
        processing: true,
        lengthChange: true,
        pageLength: 10,
        searching: true,
        ordering: true,
        filter: true,
        info: true,
        autoWidth: false,
        responsive: true,
        serverSide: true,
        responsive: true,
        language: {
            "lengthMenu": "Show _MENU_",
            paginate: {
                "next": ">",
                "previous": "<",
            }

        },

        ajax: {
            url: getApiUrl() + "Feature/GetAllFeatures",
            type: "Post",
            async: false,
            dataSrc: "data",
            error: function (xhr, textStatus, errorThrown) {
                getToasterOption()
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
                    return `<button class="btn btn-sm btn-secondary btnEdit" onclick="redirect('${row.category_id}')" >Edit</button>`

                }

            }
        ]

    });
}

function redirect(id) {
    window.location.href = window.location.origin + "/Home/AddImage/?ID=" + id
}