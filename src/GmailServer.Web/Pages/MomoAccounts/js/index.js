$(function () {
    var l = abp.localization.getResource('GmailServer');
    var createModal = new abp.ModalManager(abp.appPath + 'MomoAccounts/CreateManyModal');
    var detailModal = new abp.ModalManager(abp.appPath + 'MomoAccounts/DetailModal');
    var searchs = [
        { searchDisabled: true },
        { name: "uploadGroup", options: uploadGroupSelections },
        { name: "username" },
        { searchDisabled: true },
        { searchDisabled: true },
        { name: "status", options: momoAccountStatusSelections },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { name: "totalLinkCount", enableRangeFilter: true },
        { name: "createdTime", enableDateRangeFilter: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true }
    ];

    devmoba.datatables.enableIndividualColumnSearch("#momoAccountTable", searchs);

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
            $('select.search_c_1').chosen({ disable_search_threshold: 5, search_contains: true });
            $('select.search_c_5').chosen({ disable_search_threshold: 5, search_contains: true });
        },
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.momoAccount.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                orderable: false,
                targets: [1, 2, 3, 4],
                render: function (data, type, row, meta) {
                    return `${data}`;
                }
            },
            {
                targets: [5],
                render: function (data, type, row, meta) {
                    var status = momoAccountStatusSelections.find(x => x.value == data.toString()).text;
                    return status;
                }
            },
            {
                orderable: false,
                targets: [6, 7],
                render: function (data, type, row, meta) {
                    return `${data}`;
                }
            },
            {
                orderable: true,
                targets: [8, 9],
                render: function (data, type, row, meta) {
                    return `${data}`;
                }
            },
            {
                orderable: false,
                targets: [10, 11, 12],
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
                                text: l(`Detail`),
                                iconClass: "fa fa-info-circle",
                                visible: data => {
                                    return abp.auth.isGranted('MomoAccountGroup.MomoAccounts');
                                },
                                action: data => detailModal.open({ id: data.record.id })
                            },
                            {
                                text: l('Delete'),
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('MomoAccountGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.momoAccount.delete(data.record.id).then(() => {
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
            { data: "uploadGroup", width: "150px" },
            { data: "username", width: "220px" },
            { data: "password", width: "150px" },
            { data: "email", width: "220px" },
            { data: "status", width: "150px" },
            { data: "uDid1", width: "150px" },
            { data: "uDid2", width: "150px" },
            { data: "currentLinkCount", width: "150px" },
            { data: "totalLinkCount", width: "150px" },
            { data: "createdTime", width: "250px" },
            { data: "lastTakenTime", width: "250px" },
            { data: "lastUpdateTime", width: "250px" },
            { data: null, width: "130px" },
        ]
    });

    var dataTable = $('#momoAccountTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

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
                    gmailServer.controllers.momoAccount.deleteAll().then(() => {
                        dataTable.ajax.reload();
                    });
                }
            });
    });
});
