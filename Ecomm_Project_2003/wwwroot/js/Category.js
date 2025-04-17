var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll"
        },
        "columns": [
            {
                "data": "id",
                "render": function (data ) {
                    return `

<div class="text-primary">
<a href="/Admin/Category/Upsert/${data}" class="btn btn-info">
<i class="fas fa-edit">EDIT</i> 
</a>
</div>
<div class="btn btn-danger" onclick=Delete("/Admin/Category/Delete/${data}")>
<i class="fas fa-trash-alt">DELETE</i>
</a>
</div>
    
`;

                }
            },
            { "data": "name", "width": "30%" },
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


