var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "streetAddress", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "city", "width": "10%" },
            { "data": "postalCode", "Width": "20%" },
            { "data": "phoneNumber", "width": "10%" },
            {
                "data": "isAuthorizedCompany",
                "render": function (data) {
                    return data ?
                        `<input type="checkbox" disabled checked/>` :
                        `<input type="checkbox" disabled/>`;
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Company/Upsert/${data}" class="btn btn-info">
                                <i class="fas fa-edit"></i> EDIT
                            </a>
                            <a class="btn btn-danger" onclick="Delete('/Admin/Company/Delete/${data}')">
                                <i class="fas fa-trash-alt"></i> DELETE
                            </a>
                        </div>`;
                }
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Want To Delete Data",
        icon: "warning",
        text: "You Sure Delete Information?",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
