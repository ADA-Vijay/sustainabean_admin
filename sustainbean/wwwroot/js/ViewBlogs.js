$(document).ready(function () {
    getActiveLink('manageBlogs')
})


$(document).ready(function () {
    getActiveLink('manageCategories')
    getAllCategory()


})

function getAllCategory() {


    $("#tbl-blogs").DataTable({
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
            url: getApiUrl() + "blog/GetAllblogs",
            type: "Post",
            async: false,
            dataSrc: "data",
            error: function (xhr, textStatus, errorThrown) {
                getToasterOption()
                toastr.error("Something Went Wrong!");
            }
        },
        columns: [
            { data: "seo_title" },
            { data: "category" },
            { data: "slug" },

            { data: "description" },
            //{
            //    data: null,
            //    render: function (data, type, row, meta) {
            //        if (row.is_active == true) {

            //            return `<input class="form-check-input btnStaus" type="checkbox" value="1"  onclick="changeStatus('${row.category_id}','${row.is_active}')" checked/>`
            //        } else {
            //            return `<input class="form-check-input btnStaus" type="checkbox" value="1"   onclick="changeStatus('${row.category_id}','${row.is_active}')" />`
            //        }


            //    }
            //},
            {
                data: null,
                render: function (data, type, row, meta) {
                    return `<button class="btn btn-sm btn-secondary btnEdit" onclick="redirect('${row.blog_id}')" >Edit</button>`

                }

            }
        ]

    });
}

function redirect(id) {
    window.location.href = window.location.origin + "/Home/AddCategory/?ID=" + id
}