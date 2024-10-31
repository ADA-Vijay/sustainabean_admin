$(document).ready(function () {
    getActiveLink('manageTags')
    getAllTag()
})

function getAllTag() {


    $("#tbl-tags").DataTable({
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
            url: getApiUrl() + "Tag/GetAllTags",
            type: "Post",
            async: false,
            dataSrc: "data",
            error: function (xhr, textStatus, errorThrown) {
                getToasterOption()
                toastr.error("Something Went Wrong!");
            }
        },
        columns: [
            { data: "tag_name" },
            { data: "slug" },
            { data: "description" },
          
            {
                data: null,
                render: function (data, type, row, meta) {
                    return `<button class="btn btn-sm btn-secondary btnEdit" onclick="redirect('${row.tag_id}')" >Edit</button>`

                }

            }
        ]

    });
}

function redirect(id) {
    window.location.href = window.location.origin + "/Home/AddTags/?ID=" + id
}