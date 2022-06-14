$(function () {
    var l = abp.localization.getResource('GmailServer');
    var createModal = new abp.ModalManager(abp.appPath + 'FakeSettings/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'FakeSettings/EditModal');

    devmoba.datatables.enableIndividualColumnSearch("#fakeSettingTable", [
        { searchDisabled: true },
        { name: "deviceType" },
        { name: "version" },
        { name: "fakeVersion" },
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

        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.fakeSetting.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                orderable: false,
                targets: [1, 2, 3],
            },
            {
                targets: [4],
                rowAction: {
                    items:
                        [
                            {
                                text: l(`Edit`),
                                iconClass: "fa fa-pencil-square-o",
                                visible: data => {
                                    return abp.auth.isGranted('FakeSettingGroup.Update');
                                },
                                action: data => editModal.open({ id: data.record.id })
                            },
                            {
                                text: l('Delete'),
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('FakeSettingGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.fakeSetting.delete(data.record.id).then(() => {
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
            { data: "id", width: "80px", class: "content-cell" },
            { data: "deviceType", width: "400px", class: "content-cell" },
            { data: "version", width: "400px", class: "content-cell" },
            { data: "fakeVersion", width: "400px", class: "content-cell" },
            { data: null, width: "100px" },
        ]
    });

    var dataTable = $('#fakeSettingTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

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