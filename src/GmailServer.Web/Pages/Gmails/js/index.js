$(function () {
    var l = abp.localization.getResource('GmailShop');

    devmoba.datatables.enableIndividualColumnSearch("#gmailTable", [
        { searchDisabled: true },
        { searchDisabled: true },
        { name: "email" },
        { searchDisabled: true },
        { name: "recoveryEmail" },
        { name: "country" },
        { name: "status", options: gmailStatusSelections },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { name: "gmailTypeId", options: gmailTypeSelections },
        { name: "created", enableDateRangeFilter: true },
        { searchDisabled: true }
    ]);

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
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.gmail.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                orderable: false,
                targets: [1],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment(data);
                        data = `<span>${m.local().format('YYYY/MM/DD')}</span>`;
                    }
                    return data;
                }
            },
            {
                orderable: false,
                targets: [2, 3, 4, 5, 7, 8, 9],
            },
            {
                targets: [6],
                render: function (data, type, row, meta) {
                    var status = gmailStatusSelections.find(x => x.value == data);
                    return status.text;
                }
            },
            {
                targets: [10],
                render: function (data, type, row, meta) {
                    if (row.gmailType) {
                        return row.gmailType.name;
                    }
                    return "null";
                }
            },
            {
                orderable: false,
                targets: [11],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment(data);
                        data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD HH:mm')}</span>`;
                    }
                    return data;
                }
            },
            {
                targets: [12],
                rowAction: {
                    items:
                        [
                            {
                                text: l('Delete'),
                                visible: function (data) {
                                    return abp.auth.isGranted('GmailGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.gmail.delete(data.record.id).then(() => {
                                        abp.notify.info(l('SuccessfullyDeleted'));
                                        dataTable.ajax.reload();
                                    });
                                }
                            }
                        ]
                }
            },
        ],

        columns: [
            { data: "id", width: "100px" },
            { data: "date", width: "150px" },
            { data: "email", width: "250px" },
            { data: "password", width: "200px" },
            { data: "recoveryEmail", width: "200px" },
            { data: "country", width: "100px" },
            { data: "status", width: "150px" },
            { data: "version", width: "150px" },
            { data: "fakeVersion", width: "150px" },
            { data: "deviceType", width: "150px" },
            { data: null, width: "150px" },
            { data: "created", width: "130px" },
            { data: null, width: "100px" }
        ]
    });

    var dataTable = $('#gmailTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));
});