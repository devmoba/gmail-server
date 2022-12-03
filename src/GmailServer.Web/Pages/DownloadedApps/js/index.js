$(function () {
    var l = abp.localization.getResource('GmailServer');

    var searchs = [
        { searchDisabled: true },
        { name: "appId" },
        { name: "productId" },
        { name: "email" },
        { name: "appleId" },
        { name: "created", enableDateRangeFilter: true },
        { searchDisabled: true }
    ];

    devmoba.datatables.enableIndividualColumnSearch("#downloadedAppTable", searchs);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [50, 100, 200, 300],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        order: [[0, "desc"]],
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.downloadedApp.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                orderable: false,
                targets: [1, 2, 3, 4],
                render: function (data, type, row, meta) {
                    return data;
                }
            },
            {
                targets: [5],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment(data);
                        data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD HH:mm')}</span>`;
                    }
                    return data;
                }
            },
            {
                targets: [6],
                rowAction: {
                    items:
                        [
                            {
                                text: l('Delete'),
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('DownloadedAppGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.downloadedApp.delete(data.record.id).then(() => {
                                        abp.notify.info(l('SuccessfullyDeleted'));
                                        dataTable.ajax.reload();
                                    });
                                }
                            }
                        ]
                }
            }
        ],
        columns: [
            { data: "id", width: "100px" },
            { data: "appId", width: "150px" },
            { data: "productId", width: "300px" },
            { data: "email", width: "150px" },
            { data: "appleId", width: "150px" },
            { data: "created", width: "250px" },
            { data: null, width: "150px" }
        ]
    });

    var dataTable = $('#downloadedAppTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));
});