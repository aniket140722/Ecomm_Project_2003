var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "10%" },
            { "data": "description", "width": "20%" },
            { "data": "author", "width": "20%" },
            { "data": "isbn", "width": "10%" },
            { "data": "price", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
<div class="text-center">
<a href="/Admin/Product/Upsert/${data}" class="btn btn-info">
<i class="fas fa-edit">EDIT</i> 
</a>
<a class ="btn btn-danger" onclick=Delete("/Admin/Product/Delete/${data}")>
<i class="fas fa-trash-alt">DELETE</i>
</a>
</div>
`;

                }
            }
        ]

    })
}
function Delete(url) {
    // alert(url);
    swal({
        title: "Want To Delete Data ",
        icon: "warning",
        text: "You Sure Delete Information ",
        buttons: true,
        dangerModel: true
    }).
        then((WillDelete) => {
            if (WillDelete)
                $.ajax({
                    url: url,
                    type: "DELETE",
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message);
                            dataTable.ajax.reload();
                        }
                        else {
                            toastr.error(data.message);

                        }

                    }
                })

        })
}


