const addPayemntCompleted = [{ text: 'True', value: true }, { text: 'False', value: false }];

$(function () {
    var l = abp.localization.getResource('GmailServer');
    var createModal = new abp.ModalManager(abp.appPath + 'AppleIdNones/CreateModal');
    var searchs = [
        { searchDisabled: true },
        { name: "username", options: usernameSelections },
        { name: "email" },
        { searchDisabled: true },
        { name: "purchaseNumber", enableRangeFilter: true },
        { name: "status", options: appleIdNoneStatusSelections },
        { name: "addPaymentCompleted", options: addPayemntCompleted },
        { name: "removePaymentStatus", options: removePaymentStatusSelections },
        { searchDisabled: true },
        { searchDisabled: true },
        { name: "takenOutNumber", enableRangeFilter: true },
        { searchDisabled: true },
        { name: "created", enableDateRangeFilter: true },
        { searchDisabled: true }
    ];

    devmoba.datatables.enableIndividualColumnSearch("#appleIdNoneTable", searchs);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [100, 200, 300, 500],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        order: [[0, "desc"]],
        initComplete: () => {
            $('select.search_c_4').chosen({ disable_search_threshold: 5, search_contains: true });
            $('select.search_c_5').chosen({ disable_search_threshold: 5, search_contains: true });
            $('select.search_c_6').chosen({ disable_search_threshold: 5, search_contains: true });
        },
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.appleIdNone.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                orderable: false,
                targets: [2],
                render: function (data, type, row, meta) {
                    return `<span class="text-ellipsis">${data}</span>`;
                }
            },
            {
                targets: [4],
                render: function (data, type, row, meta) {
                    if (abp.auth.isGranted('AppleIdNoneGroup.PurchaseNumber')) {
                        return data;
                    }
                    return `NA`;
                }
            },
            {
                targets: [5],
                render: function (data, type, row, meta) {
                    var status = appleIdNoneStatusSelections.find(x => x.value == data.toString()).text;
                    return status;
                }
            },
            {
                targets: [7],
                render: function (data, type, row, meta) {
                    var status = removePaymentStatusSelections.find(x => x.value == data.toString()).text;
                    return status;
                }
            },
            {
                orderable: false,
                targets: [1, 3, 6],
                render: function (data, type, row, meta) {
                    return `${data}`;
                }
            },
            {
                orderable: false,
                targets: [8, 9, 11, 12],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment(data);
                        data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD HH:mm')}</span>`;
                    }
                    return data;
                }
            },
            {
                targets: [13],
                rowAction: {
                    items:
                        [
                            {
                                text: l('Delete'),
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('AppleIdNoneGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.appleIdNone.delete(data.record.id).then(() => {
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
            { data: "username", width: "150px" },
            { data: "email", width: "220px" },
            { data: "password", width: "150px" },
            { data: "purchaseNumber", width: "150px" },
            { data: "status", width: "150px" },
            { data: "addPaymentCompleted", width: "150px" },
            { data: "removePaymentStatus", width: "150px" },
            { data: "removeTakenTime", width: "250px" },
            { data: "removeUpdateTime", width: "250px" },
            { data: "takenOutNumber", width: "250px" },
            { data: "takenTime", width: "250px" },
            { data: "created", width: "250px" },
            { data: null, width: "130px" },
        ]
    });

    var dataTable = $('#appleIdNoneTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    createModal.onResult(() => {
        dataTable.ajax.reload();
    });

    $('#createBtn').click((e) => {
        e.preventDefault();
        createModal.open();
    });

    $('#btnRemoveAll').click((e) => {
        e.preventDefault();
        abp.message.confirm('Are you sure to remove all accounts?')
            .then(function (confirmed) {
                if (confirmed) {
                    gmailServer.controllers.appleIdNone.deleteAll().then(() => {
                        dataTable.ajax.reload();
                    });
                }
            });
    });
});