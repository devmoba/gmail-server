$(function () {
    var l = abp.localization.getResource('GmailServer');
    gmailServer.controllers.momoAccount.getUploadGroupSelection().done((uploadGroups) => {
        var searchs = [
            { searchDisabled: true },
            { name: "createdTime", enableDateRangeFilter: true },
            { name: "uploadGroup", options: uploadGroups },
            { searchDisabled: true },
            { searchDisabled: true },
            { searchDisabled: true },
            { searchDisabled: true },
            { searchDisabled: true },
            { searchDisabled: true },
        ];

        devmoba.datatables.enableIndividualColumnSearch("#momoAccountStatisticTable", searchs);

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
            ajax: abp.libs.datatables.createAjax(gmailServer.controllers.momoAccount.getStatistic, () => {
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
                    targets: [2, 3, 4, 5, 6, 7, 8],
                    render: function (data, type, row, meta) {
                        return data;
                    }
                }
            ],
            columns: [
                { data: null, width: "100px" },
                { data: "createdTime", width: "200px" },
                { data: "uploadGroup", width: "200px" },
                { data: "total", width: "300px" },
                { data: "notUse", width: "150px" },
                { data: "inUse", width: "150px" },
                { data: "lock", width: "150px" },
                { data: "wrongPassword", width: "150px" },
                { data: "unknown", width: "150px" }
            ]
        });

        $('#momoAccountStatisticTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));
    });
});