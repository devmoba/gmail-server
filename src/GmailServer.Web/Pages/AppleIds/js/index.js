$(function () {
    var l = abp.localization.getResource('GmailServer');
    var createModal = new abp.ModalManager(abp.appPath + 'AppleIds/CreateModal');
    var resetStatusModal = new abp.ModalManager(abp.appPath + 'AppleIds/ResetStatusModal');
   
    var searchs = [
        { searchDisabled: true },
        { name: "username", options: usernameSelections },
        { name: "email" },
        { searchDisabled: true },
        { name: "status", options: appleIdStatusSelections },
        { searchDisabled: true },
        { name: "created", enableDateRangeFilter: true },
        { searchDisabled: true },
        { searchDisabled: true }
    ];
    if (isRoleNameAppleIdMember) {
        searchs[1] = { searchDisabled: true };
    }
    devmoba.datatables.enableIndividualColumnSearch("#appleIdTable", searchs);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [150, 300, 500, 1000, 2000],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        order: [[0, "desc"]],
        initComplete: () => {
            $('select.search_c_1').chosen({ disable_search_threshold: 5, search_contains: true });
            $('select.search_c_4').chosen({ disable_search_threshold: 5, search_contains: true });
        },
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.appleId.getList, () => {
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
                    var status = appleIdStatusSelections.find(x => x.value == data.toString()).text;
                    return status;
                }
            },
            {
                targets: [5, 6, 7],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment(data);
                        data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD HH:mm')}</span>`;
                    }
                    return data;
                }
            },
            {
                targets: [8],
                rowAction: {
                    items:
                        [
                            {
                                text: l('Delete'),
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('AppleIdGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.appleId.delete(data.record.id).then(() => {
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
            { data: "email", width: "300px" },
            { data: "password", width: "150px" },
            { data: "status", width: "150px" },
            { data: "takenTime", width: "250px" },
            { data: "created", width: "250px" },
            { data: "updated", width: "250px" },
            { data: null, width: "100px" },
        ]
    });

    var dataTable = $('#appleIdTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    createModal.onResult(() => {
        dataTable.ajax.reload();
    });

    $('#createBtn').click((e) => {
        e.preventDefault();
        createModal.open();
    });

    $('#btnResetStatus').click((e) => {
        e.preventDefault();
        resetStatusModal.open();
    });

    resetStatusModal.onOpen(() => {
        var viewModel = new ResetStatusViewModel(appleIdStatusSelections);
        ko.applyBindings(viewModel);
    });

    $('#btnRemoveAll').click((e) => {
        e.preventDefault();
        abp.message.confirm('Are you sure to remove all recovery emails?')
            .then(function (confirmed) {
                if (confirmed) {
                    gmailServer.controllers.appleId.deleteAll().then(() => {
                        dataTable.ajax.reload();
                    });
                }
            });
    });
});