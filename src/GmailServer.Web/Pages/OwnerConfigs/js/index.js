$(function () {
    var l = abp.localization.getResource('GmailServer');
    var createModal = new abp.ModalManager(abp.appPath + 'OwnerConfigs/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'OwnerConfigs/EditModal');

    devmoba.datatables.enableIndividualColumnSearch("#ownerConfigTable", [
        { searchDisabled: true },
        { name: "key" },
        { name: "value" },
        { searchDisabled: true }
    ]);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [20, 30, 50, 100],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        order: [[0, "desc"]],

        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.ownerConfig.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                orderable: false,
                targets: [1, 2],
                render: function (data, type, row, meta) {
                    return `<span class="text-ellipsis">${data}</span>`;
                }
            },
            {
                targets: [3],
                rowAction: {
                    items:
                        [
                            {
                                text: l(`Edit`),
                                iconClass: "fa fa-pencil-square-o",
                                visible: data => {
                                    return abp.auth.isGranted('OwnerConfigGroup.Update');
                                },
                                action: data => editModal.open({ id: data.record.id })
                            },
                            {
                                text: l('Delete'),
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('OwnerConfigGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.ownerConfig.delete(data.record.id).then(() => {
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
            { data: "key" },
            { data: "value" },
            { data: null, width: "100px" },
        ]
    });

    var dataTable = $('#ownerConfigTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

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