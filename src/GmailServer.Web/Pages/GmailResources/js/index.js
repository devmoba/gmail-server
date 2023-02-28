$(function () {
    var l = abp.localization.getResource('GmailServer');
    var createModal = new abp.ModalManager(abp.appPath + 'GmailResources/CreateModal');
    var reupModal = new abp.ModalManager(abp.appPath + 'GmailResources/ReupModal');

    var searchs = [
        { searchDisabled: true },
        { name: "username", options: usernameSelections },
        { name: "email" },
        { searchDisabled: true },
        { searchDisabled: true },
        { name: "status", options: gmailResourceStatusSelections },
        { name: "premiumType", options: premiumTypeSelections },
        { searchDisabled: true },
        { searchDisabled: true },
        { name: "created", enableDateRangeFilter: true },
        { searchDisabled: true },
        { searchDisabled: true }
    ];

    if (isRoleNameAppleIdMember) {
        searchs[1] = { searchDisabled: true };
    }
    devmoba.datatables.enableIndividualColumnSearch("#gmailResourceTable", searchs);

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
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.gmailResource.getList, () => {
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
                targets: [5],
                render: function (data, type, row, meta) {
                    var status = gmailResourceStatusSelections.find(x => x.value == data).text;
                    return status;
                }
            },
            {
                targets: [6],
                render: function (data, type, row, meta) {
                    var premiumType = premiumTypeSelections.find(x => x.value == data).text;
                    return premiumType;
                }
            },
            {
                targets: [7, 8, 9, 10],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment(data);
                        data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD HH:mm')}</span>`;
                    }
                    return data;
                }
            },
            {
                targets: [11],
                rowAction: {
                    items:
                        [
                            {
                                text: l('Delete'),
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('GmailResourceGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.gmailResource.delete(data.record.id).then(() => {
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
            { data: "email", width: "250px" },
            { data: "password", width: "150px" },
            { data: "recoveryEmail", width: "300px" },
            { data: "status", width: "150px" },
            { data: "premiumType", width: "150px" },
            { data: "updatedPremium", width: "250px" },
            { data: "takenTime", width: "250px" },
            { data: "created", width: "250px" },
            { data: "updated", width: "250px" },
            { data: null, width: "100px" },
        ]
    });

    var dataTable = $('#gmailResourceTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    createModal.onResult(() => {
        dataTable.ajax.reload();
    });

    $('#createBtn').click((e) => {
        e.preventDefault();
        createModal.open();
    });

    $('#reupBtn').click((e) => {
        e.preventDefault();
        reupModal.open();
    });

    $('#btnResetStatus').click((e) => {
        e.preventDefault();
        resetStatusModal.open();
    });

    $('#btnRemoveAll').click((e) => {
        e.preventDefault();
        abp.message.confirm('Are you sure to remove all recovery emails?')
            .then(function (confirmed) {
                if (confirmed) {
                    gmailServer.controllers.gmailResource.deleteAll().then(() => {
                        dataTable.ajax.reload();
                    });
                }
            });
    });
});