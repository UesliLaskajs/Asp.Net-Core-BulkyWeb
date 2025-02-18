$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    } else if (url.includes("pending")) {
        loadDataTable("pending");
    } else if (url.includes("completed")) {
        loadDataTable("completed");
    } else if (url.includes("approved")) {
        loadDataTable("approved");
    } else if (url.includes("all")) {
        loadDataTable("all");
    } else {
        loadDataTable();  // Default case
    }
});
function loadDataTable(status) {
    $('#myOrderTable').DataTable({
        "ajax": {
            url: '/Admin/order/getall?status=' + status, // API endpoint to fetch data from
            type: 'GET',
            dataSrc: 'data' // The response is wrapped in a "data" property
        },
        "columns": [
            { "data": "id", "width": "15%" },         // Column for title
            { "data": "name", "width": "15%" },          // Column for ISBN
            { "data": "phoneNumber", "width": "15%" },         // Column for price
            { "data": "orderStatus", "width": "15%" },        // Column for author
            { "data": "applicationUser.email", "width": "15%" },    // Column for categoryId
            { "data": "orderTotal", "width": "15%" },   // Column for category name
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2">
                                    <i class="bi bi-pencil-square"></i> Edit
                                </a>
                               
                            </div>`;
                },
                "width": "25%"
            }
        ]
    });
}
