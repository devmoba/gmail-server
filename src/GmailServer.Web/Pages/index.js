$(function () {
    gmailServer.controllers.gmail.getReportbyStatus().done((res) => {
        var chart = new CanvasJS.Chart("chartContainer", {
            exportEnabled: true,
            animationEnabled: true,
            title: {
                text: "Gmail Status Chart"
            },
            legend: {
                cursor: "pointer",
                itemclick: explodePie
            },
            data: [{
                type: "pie",
                showInLegend: true,
                toolTipContent: `{name}: <strong>{y}</strong>`,
                indexLabel: "{name} - {y}",
                dataPoints: res.statusPoints
            }]
        });
        chart.render();
    });

    devmoba.datatables.enableIndividualColumnSearch("#gmailReportTable", [
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
    ]);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [15, 30, 60, 100, 120],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,

        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.gmail.getGmailReports, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                target: [0],
                orderable: false,
                render: function (data, type, row, meta) {
                    return `${meta.row + 1}`;
                }
            },
            {
                targets: [1],
                orderable: false,
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment(data);
                        data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD')}</span>`;
                    }
                    return data;
                }
            },
            {
                targets: [2, 3, 4, 5, 6, 7, 8, 9],
                orderable: false
            }
        ],
        columns: [
            { data: null, width: "100px" },
            { data: "created", width: "350px" },
            { data: "totalDaily", width: "150px" },
            { data: "good", width: "150px" },
            { data: "verify", width: "150px" },
            { data: "checking", width: "150px" },
            { data: "uncheck", width: "150px" },
            { data: "disable", width: "150px" },
            { data: "notexist", width: "150px" },
            { data: "unknown", width: "150px" }
        ]
    });

    var dataTable = $('#gmailReportTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    function explodePie(e) {
        if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
            e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
        } else {
            e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
        }
        e.chart.render();

    }
});