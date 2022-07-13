$(function () {
    var l = abp.localization.getResource('GmailShop');

    devmoba.datatables.enableIndividualColumnSearch("#checkerTable", [
        { searchDisabled: true },
        { name: "checkerId" },
        { name: "checkerIP" },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true }
    ]);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [20, 30, 50, 100, 200],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        order: [[0, "desc"]],
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.checker.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                orderable: false,
                targets: [1, 2],
            },
            {
                targets: [7],
                render: function (data, type, row, meta) {
                    if (data == 0 && type === 'display') {
                        return `<span style="color:red;"><i class="fa fa-circle-o" aria-hidden="true"></i>&nbsp;<b>Offline</b></span>`;
                    }
                    return `<span style="color:green;"><i class="fa fa-circle" aria-hidden="true"></i>&nbsp;<b>Online</b></span>`;
                }
            },
            {
                targets: [8, 9],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment.utc(data);
                        data = `<span title="${m.local().format('YYYY/MM/DD HH:mm')}">${m.fromNow()}</span>`;
                    }
                    return data;
                }
            }
        ],

        columns: [
            { data: "id", width: "100px" },
            { data: "checkerId", width: "200px" },
            { data: "checkerIP", width: "200px" },
            { data: "freeRam", width: "150px" },
            { data: "totalRam", width: "150px" },
            { data: "usingThread", width: "150px" },
            { data: "maxThread", width: "150px" },
            { data: "status", width: "150px" },
            { data: "lastCheck", width: "200px" },
            { data: "created", width: "200px" }
        ]
    });

    var dataTable = $('#checkerTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));
});