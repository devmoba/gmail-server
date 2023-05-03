$(function () {
    var searchs = [
        { searchDisabled: true },
        { name: "created", enableDateRangeFilter: true },
        { searchDisabled: true }
    ];

    devmoba.datatables.enableIndividualColumnSearch("#appleIdRawStatistic", searchs);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [30, 50, 100, 150, 250],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        ordering: false,
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.appleIdRaw.getAppleIdRawStatisticDaily, () => {
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
                targets: [2],
                render: function (data, type, row, meta) {
                    return data;
                }
            }
        ],
        columns: [
            { data: null, width: "100px" },
            { data: "created", width: "200px" },
            { data: "count", width: "200px" }
        ]
    });

    $('#appleIdRawStatistic').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));
});