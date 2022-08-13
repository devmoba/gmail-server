$(function () {
    var l = abp.localization.getResource('GmailServer');
    var createModal = new abp.ModalManager(abp.appPath + 'RecoveryEmails/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'RecoveryEmails/EditModal');

    devmoba.datatables.enableIndividualColumnSearch("#recoveryEmailTable", [
        { searchDisabled: true },
        { name: "username" },
        { name: "status" },
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

        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.recoveryEmail.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                targets: [3],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment.utc(data);
                        data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD HH:mm')}</span>`;
                    }
                    return data;
                }
            },
            {
                orderable: false,
                targets: [4],
                render: function (data, type, row, meta) {
                    return `<span class="text-ellipsis">${data}</span>`;
                }
            },
            {
                targets: [5],
                rowAction: {
                    items:
                        [
                            {
                                text: l(`Edit`),
                                iconClass: "fa fa-pencil-square-o",
                                visible: data => {
                                    return abp.auth.isGranted('RecoveryEmailGroup.Update');
                                },
                                action: data => editModal.open({ id: data.record.id })
                            },
                            {
                                text: l('Delete'),
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('RecoveryEmailGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.recoveryEmail.delete(data.record.id).then(() => {
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
            { data: "status", width: "150px" },
            { data: "created", width: "250px" },
            { data: "emails" },
            { data: null, width: "100px" },
        ]
    });

    var dataTable = $('#recoveryEmailTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    createModal.onResult(() => {
        dataTable.ajax.reload();
    });

    editModal.onResult(() => {
        dataTable.ajax.reload();
    });

    $('#createBtn').click((e) => {
        e.preventDefault();
        createModal.open();
    });
});