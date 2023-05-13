$(function () {

    var l = abp.localization.getResource('GmailServer');
    //gmailServer.controllers.gmailResource.getUsernameSelection()
    //    .then((usernames) => {
    //        window.localStorage.setItem("GmailResource_UsernameSelections", JSON.stringify(usernames));
    //    });

    //var usernames = JSON.parse(window.localStorage.getItem("GmailResource_UsernameSelections"));

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
        { searchDisabled: true } 
    ];

    devmoba.datatables.enableIndividualColumnSearch("#gmailResourceStatisticTable", searchs);

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
            $('select.search_c_2').chosen({ disable_search_threshold: 5, search_contains: true });
        },
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.gmailResource.getStatistic, () => {
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
                targets: [2, 3, 4, 5, 6, 7, 8, 9, 10],
                render: function (data, type, row, meta) {
                    return data;
                }
            },
            {
                targets: [11],
                rowAction: {
                    items:
                        [
                            {
                                text: l('Daily'),
                                iconClass: "fa fa-calendar-o",
                                visible: function (data) {
                                    return abp.auth.isGranted('GmailResourceGroup.StatisticDaily');
                                },
                                action: data => window.open(`/GmailResources/StatisticDaily?Username=${data.record.username}`)
                            }
                        ]
                }
            },
        ],
        columns: [
            { data: null, width: "100px" },
            { data: "created", width: "200px" },
            { data: "username", width: "200px" },
            { data: "total", width: "150px" },
            { data: "ready", width: "150px" },
            { data: "success", width: "150px" },
            { data: "pending", width: "150px" },
            { data: "used", width: "150px" },
            { data: "failed", width: "150px" },
            { data: "error", width: "150px" },
            { data: "unknown", width: "150px" },
            { data: null, width: "100px" },
        ]
    });

    var datatable = $('#gmailResourceStatisticTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    //function explodePie(e) {
    //    if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
    //        e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
    //    } else {
    //        e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
    //    }
    //    e.chart.render();

    //}
});

//function StatisticViewModel() {
//    var self = this;
//    self.usernameSelections = ko.observableArray([]);
//    self.getUsernameSelection = ko.computed(function () {
//        gmailServer.controllers.gmailResource.getUsernameSelection().done(function (result) {
//            result.forEach((item) => {
//                self.usernameSelections.push({
//                    text: item,
//                    value: item,
//                    disable: false,
//                    group: null,
//                    selected: false
//                });
//            })
//        });
//    });
//}
