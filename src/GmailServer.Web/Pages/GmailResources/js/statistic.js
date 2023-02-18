$(function () {
    gmailServer.controllers.gmailResource.getStatisticByUsername().done((res) => {
        var chart = new CanvasJS.Chart("chartContainer", {
            exportEnabled: false,
            animationEnabled: true,
            title: {
                text: "Gmail Resource Statistics"
            },
            legend: {
                cursor: "pointer",
                itemclick: explodePie
            },
            data: [{
                type: "pie",
                showInLegend: false,
                toolTipContent: `{name}: <strong>{y}</strong>`,
                indexLabel: "{name} - {y}",
                dataPoints: res.statusPoints
            }]
        });
        chart.render();
    });

    var l = abp.localization.getResource('GmailServer');
    //var viewModel = new StatisticViewModel();
    //ko.applyBindings(viewModel);
    //console.log(viewModel.usernameSelections());

    var searchs = [
        { searchDisabled: true },
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
        order: [[1, "desc"]],
        initComplete: () => {
            $('select.search_c_1').chosen({ disable_search_threshold: 5, search_contains: true });
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
                    return data;
                }
            },
            {
                orderable: false,
                targets: [2, 3, 4, 5, 6, 7, 8, 9],
                render: function (data, type, row, meta) {
                    return data;
                }
            },
            {
                targets: [10],
                rowAction: {
                    items:
                        [
                            {
                                text: l('Daily'),
                                iconClass: "fa fa-calendar-o",
                                visible: function (data) {
                                    return abp.auth.isGranted('GmailResourceGroup.Statistic');
                                },
                                action: data => window.open(`/GmailResources/StatisticDaily?Username=${data.record.username}`)
                            }
                        ]
                }
            },
        ],
        columns: [
            { data: null, width: "100px" },
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

    var dataTable = $('#gmailResourceStatisticTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    function explodePie(e) {
        if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
            e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
        } else {
            e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
        }
        e.chart.render();

    }
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
