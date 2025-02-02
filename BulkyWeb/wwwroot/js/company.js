$(document).ready(function () {
    console.log("🚀 company.js is running automatically!");
    loadCompanyTable();
});

function loadCompanyTable() {
    console.log("🔄 Running loadCompanyTable()...");

    if ($("#myTableCompany").length === 0) {
        console.error("❌ ERROR: #myTableCompany not found in the DOM.");
        return;
    }

    console.log("✅ Table #myTableCompany found, initializing DataTable...");

    $('#myTableCompany').DataTable({
        "ajax": {
            url: '/Admin/Company/getall',
            type: 'GET',
            dataSrc: 'data'
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAdress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "postalCode", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/company/upsert/${data}" class="btn btn-primary mx-2">
                                    <i class="bi bi-pencil-square"></i> Edit
                                </a>
                                <a href="/admin/company/delete/${data}" class="btn btn-danger mx-2">
                                    <i class="bi bi-trash-fill"></i> Delete
                                </a>
                            </div>`;
                },
                "width": "25%"
            }
        ],
        "initComplete": function () {
            console.log("✅ Company DataTable Loaded Successfully!");
        }
    });
}
