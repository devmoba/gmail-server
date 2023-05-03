$(function () {
    var searchs = [
        { searchDisabled: true },
        { name: "createdTime", enableDateRangeFilter: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true }
    ];

    devmoba.datatables.enableIndividualColumnSearch("#statisticByAddPaymentStatusTable", searchs);

    var datatableConfigAddPaymentStatus = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [30, 50, 100, 150, 250],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        //order: [[1, "desc"]],
        ordering: false,
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.appleOrder.getStatisticByAddPaymentStatus, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
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
                targets: [2, 3, 4, 5, 6, 7],
                render: function (data, type, row, meta) {
                    return data;
                }
            }
        ],
        columns: [
            { data: null, width: "100px" },
            { data: "createdTime", width: "200px" },
            { data: "total", width: "200px" },
            { data: "none", width: "150px" },
            { data: "inUse", width: "150px" },
            { data: "expired", width: "150px" },
            { data: "error", width: "150px" },
            { data: "completed", width: "150px" }
        ]
    });

    $('#statisticByAddPaymentStatusTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfigAddPaymentStatus));
});