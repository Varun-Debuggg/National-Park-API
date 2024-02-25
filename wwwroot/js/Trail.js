var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url":"/Trail/GetAll"
        },
        "columns": [
            { "data": "nationalPark.name", "width":"20%" },
            { "data": "name", "width":"20%" },
            { "data": "distance", "width":"20%" },
            { "data": "elevation", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `

                         <div class="text-center">
                           <a href="Trail/Upsert/${data}" class="btn btn-info">
                            <i class="fas fa-edit"></i>
                            </a>
                            <a class="btn btn-danger" onclick=Delete("Trail/Delete/${data}")>
                            <i class="fas fa-trash"></i>


                        `;
                }
            }
        ]
    })
}

function Delete(url) {
    swal({
        title: "Want to delete the data",
        text: "Delete information ",
        icon: "error",
        buttons: true,
        dangerModel: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        swal("Data Has been Deleted Successfully!", {
                            icon: "success",
                        });
                        dataTable.ajax.reload();
                        //toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}