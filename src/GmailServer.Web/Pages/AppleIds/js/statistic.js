$(function () {
    var l = abp.localization.getResource('GmailServer');
    var searchs = [
        { searchDisabled: true },
        { name: "created", enableDateRangeFilter: true },
        { name: "username", options: usernameSelections },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
    ];

    devmoba.datatables.enableIndividualColumnSearch("#appleIdStatisticTable", searchs);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
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
        initComplete: () => {
            $('select.search_c_2').chosen({ disable_search_threshold: 7, search_contains: true });
        },
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.appleId.getStatistic, () => {
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
                targets: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17],
                render: function (data, type, row, meta) {
                    return data;
                }
            },
            {
                targets: [18],
                rowAction: {
                    items:
                        [
                            {
                                text: l('Daily'),
                                iconClass: "fa fa-calendar-o",
                                visible: function (data) {
                                    return abp.auth.isGranted('AppleIdGroup.StatisticDaily');
                                },
                                action: data => window.open(`/AppleIds/StatisticDaily?Username=${data.record.username}`)
                            }
                        ]
                }
            },
        ],
        columns: [
            { data: null, width: "100px" },
            { data: "created", width: "200px" },
            { data: "username", width: "200px" },
            { data: "total", width: "300px" },
            { data: "totalPurchaseNumber", width: "150px" },
            { data: "ready", width: "150px" },
            { data: "completed1", width: "150px" },
            { data: "completed2", width: "150px" },
            { data: "completed3", width: "150px" },
            { data: "completed4", width: "150px" },
            { data: "pending", width: "150px" },
            { data: "wrongPass", width: "100px" },
            { data: "subed", width: "100px" },
            { data: "locked1", width: "100px" },
            { data: "locked2", width: "100px" },
            { data: "review", width: "100px" },
            { data: "error", width: "100px" },
            { data: "unknown", width: "150px" },
            { data: null, width: "100px" },
        ]
    });

    var dataTable = $('#appleIdStatisticTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    //function explodePie(e) {
    //    if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
    //        e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
    //    } else {
    //        e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
    //    }
    //    e.chart.render();

    //}
});