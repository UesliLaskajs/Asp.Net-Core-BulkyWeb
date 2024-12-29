$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#myTable').DataTable({
        "ajax": {
            url: '/Admin/Product/getall', // API endpoint to fetch data from
            type: 'GET',
            dataSrc: 'data' // The response is wrapped in a "data" property
        },
        "columns": [
            { "data": "title", "width": "15%" },         // Column for title
            { "data": "isbn", "width": "15%" },          // Column for ISBN
            { "data": "price", "width": "15%" },         // Column for price
            { "data": "author", "width": "15%" },        // Column for author
            { "data": "categoryId", "width": "15%" },    // Column for categoryId
            { "data": "categoryName", "width": "15%" },   // Column for category name
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/product/upsert/${data}" class="btn btn-primary mx-2">
                                    <i class="bi bi-pencil-square"></i> Edit
                                </a>
                                <a href="/admin/product/delete/${data}" class="btn btn-danger mx-2">
                                    <i class="bi bi-trash-fill"></i> Delete
                                </a>
                            </div>`;
                },
                "width": "25%"
            }
        ]
    });
}
