$(function () {
    var l = abp.localization.getResource('GmailServer');
    var searchs = [
        { searchDisabled: true },
        { name: "orderID" },
        { searchDisabled: true },
        { name: "linkStatus", options: linkStatusSelections },
        { searchDisabled: true },
        { searchDisabled: true },
        { name: "addPaymentStatus", options: addPaymentStatusSelections },
        { searchDisabled: true },
        { searchDisabled: true },
        { name: "momoAccount" },
        { name: "appleID" },
        { name: "createdTime", enableDateRangeFilter: true },
        { searchDisabled: true }
    ];

    devmoba.datatables.enableIndividualColumnSearch("#appleOrderTable", searchs);

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
            $('select.search_c_3').chosen({ disable_search_threshold: 5, search_contains: true });
            $('select.search_c_6').chosen({ disable_search_threshold: 5, search_contains: true });
        },
        ajax: abp.libs.datatables.createAjax(gmailServer.controllers.appleOrder.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                orderable: false,
                targets: [1, 9, 10],
                render: function (data, type, row, meta) {
                    return `${data}`;
                }
            },
            {
                orderable: false,
                targets: [2],
                render: function (data, type, row, meta) {
                    return `<span class="text-ellipsis">${data}</span>`;
                }
            },
            {
                targets: [3],
                render: function (data, type, row, meta) {
                    var status = linkStatusSelections.find(x => x.value == data.toString()).text;
                    return status;
                }
            },
            {
                targets: [6],
                render: function (data, type, row, meta) {
                    var status = addPaymentStatusSelections.find(x => x.value == data.toString()).text;
                    return status;
                }
            },
            {
                orderable: false,
                targets: [4, 5, 7, 8, 11],
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
                                iconClass: "fas fa-trash-alt",
                                visible: function (data) {
                                    return abp.auth.isGranted('AppleOrderGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: data => {
                                    gmailServer.controllers.appleOrder.delete(data.record.id).then(() => {
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
            { data: "orderID", width: "150px" },
            { data: "urlPayment", width: "400px" },
            { data: "linkStatus", width: "150px" },
            { data: "linkTakenTime", width: "250px" },
            { data: "linkCompletedTime", width: "250px" },
            { data: "addPaymentStatus", width: "150px" },
            { data: "addPaymentTakenTime", width: "250px" },
            { data: "addPaymentCompletedTime", width: "250px" },
            { data: "momoAccount", width: "150px" },
            { data: "appleId", width: "250px" },
            { data: "createdTime", width: "250px" },
            { data: null, width: "130px" },
        ]
    });

    var dataTable = $('#appleOrderTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

});
