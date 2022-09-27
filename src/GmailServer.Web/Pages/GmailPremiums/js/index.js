const Status = [
    { text: 'Ready', value: 0 },
    { text: 'Completed', value: 1 }
];
$(function () {
    var l = abp.localization.getResource('GmailServer');
    var createModal = new abp.ModalManager(abp.appPath + 'GmailPremiums/CreateModal');

    devmoba.datatables.enableIndividualColumnSearch("#gmailPremiumTable", [
        { searchDisabled: true },
        { name: "username" },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { name: "status", options: gmailPremiumStatusSelections },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true }
    ]);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [15, 25, 50, 100],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        order: [[0, "desc"]],

        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.gmailPremium.getList, () => {
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
                    var status = gmailPremiumStatusSelections.find((status) => status.value == data.toString());
                    return status.text;
                }
            },
            {
                targets: [6, 7, 8],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment(data);
                        data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD HH:mm')}</span>`;
                    }
                    return data;
                }
            },
            {
                targets: [9],
                rowAction: {
                    items:
                        [
                            {
                                text: l('Delete'),
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('GmailPremiumGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.gmailPremium.delete(data.record.id).then(() => {
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
            { data: "recoveryEmail", width: "300px" },
            { data: "status", width: "150px" },
            { data: "takenTime", width: "250px" },
            { data: "created", width: "250px" },
            { data: "updated", width: "250px" },
            { data: null, width: "100px" },
        ]
    });

    var dataTable = $('#gmailPremiumTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    createModal.onResult(() => {
        dataTable.ajax.reload();
    });

    $('#createBtn').click((e) => {
        e.preventDefault();
        createModal.open();
    });

    $('#btnRemoveAll').click((e) => {
        e.preventDefault();
        abp.message.confirm('Are you sure to remove all recovery emails?')
            .then(function (confirmed) {
                if (confirmed) {
                    gmailServer.controllers.gmailPremium.deleteAll().then(() => {
                        dataTable.ajax.reload();
                    });
                }
            });
    });
});