$(function () {
    var l = abp.localization.getResource('GmailServer');

    var searchs = [
        { searchDisabled: true },
        { name: "created", enableDateRangeFilter: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true }
    ];

    devmoba.datatables.enableIndividualColumnSearch("#gmailResourceStatisticDailyTable", searchs);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [30, 50, 100, 150, 250],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        order: [[1, "desc"]],
        initComplete: () => {

        },
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.gmailResource.getStatisticDaily, () => {
            var res = devmoba.datatables.searchHelper.getSearchConditions();
            res.username = usernameParam;
            return res;
        }),
        columnDefs: [
            {
                orderable: false,
                targets: [0],
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: true,
                targets: [1],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment(data);
                        data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD')}</span>`;
                    }
                    return data;
                }
            },
            {
                orderable: false,
                targets: [2, 3, 4, 5, 6, 7, 8, 9],
                render: function (data, type, row, meta) {
                    return data;
                }
            }
        ],
        columns: [
            { data: null, width: "100px" },
            { data: "created", width: "250px" },
            { data: "total", width: "150px" },
            { data: "ready", width: "150px" },
            { data: "success", width: "150px" },
            { data: "pending", width: "150px" },
            { data: "used", width: "150px" },
            { data: "failed", width: "150px" },
            { data: "error", width: "150px" },
            { data: "unknown", width: "150px" },
        ]
    });

    var dataTable = $('#gmailResourceStatisticDailyTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));
});