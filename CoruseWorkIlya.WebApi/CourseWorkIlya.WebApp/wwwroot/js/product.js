$(document).ready(function () {
    console.log("Document is ready");
    loadDataTable();
});

function loadDataTable() {
    console.log("Initializing DataTable");
    $("#tblData").DataTable({
        "ajax": {
            "url": "/Home/GetProductList",
            "type": "GET",
            "dataSrc": "data",
            "error": function (xhr, error, thrown) {
                console.log("Error in API call");
                console.log(xhr);
                console.log(error);
                console.log(thrown);
            }
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "name", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data, type, row) {
                    return `<div class="text-center">
                                <a href="/Home/Details/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                    <i class="bi bi-search"></i>
                                </a>
                            </div>`;
                },
                "width": "5%",
                "orderable": false
            }
        ]
    });
}