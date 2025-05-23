﻿var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tbldata').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        //User Locked
                        return`
                        <div class="text-center">
                            <a class="btn btn-danger" onclick=LockUnlock('${data.id}')>
                            Unlock
                        </a>
                            </div >`
                          ;
                    }
                    else {
                        
                        return`
                        <div class="text-center">
                            <a class="btn btn-success" onclick=LockUnlock('${data.id}')>
                            Lock
                        </a>
                            </div >`
                          ;
                           

                    }
                }

            }
        ]

    })

}
function LockUnlock(id) {
    $.ajax({
        url: "/Admin/User/LockUnlock",
        type: "Post",
        data: JSON.stringify(id),
        contentType: "application/json",
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
}