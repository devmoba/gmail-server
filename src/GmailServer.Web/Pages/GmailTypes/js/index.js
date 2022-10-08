$(function () {
    var l = abp.localization.getResource('GmailServer');
    var createModal = new abp.ModalManager(abp.appPath + 'GmailTypes/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'GmailTypes/EditModal');

    devmoba.datatables.enableIndividualColumnSearch("#gmailTypeTable", [
        { searchDisabled: true },
        { name: "name" },
        { name: "deviceType" },
        { name: "version" },
        { name: "fakeVersion" },
        { name: "country" },
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

        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.gmailType.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                orderable: false,
                targets: [1, 2, 3, 4, 5],
                render: function (data, type, row, meta) {
                    return data;
                }
            },
            {
                targets: [6],
                rowAction: {
                    items:
                        [
                            {
                                text: l(`Edit`),
                                iconClass: "fa fa-pencil-square-o",
                                visible: data => {
                                    return abp.auth.isGranted('GmailTypeGroup.Update');
                                },
                                action: data => editModal.open({ id: data.record.id })
                            },
                            {
                                text: l('Delete'),
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('GmailTypeGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.gmailType.delete(data.record.id).then(() => {
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
            { data: "name" },
            { data: "deviceType" },
            { data: "version" },
            { data: "fakeVersion" },
            { data: "country" },
            { data: null, width: "100px" },
        ]
    });

    var dataTable = $('#gmailTypeTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

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