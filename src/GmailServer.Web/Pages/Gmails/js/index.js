$(function () {

    $(function () {
        var l = abp.localization.getResource('GmailShop');

        devmoba.datatables.enableIndividualColumnSearch("#gmailTable", [
            { searchDisabled: true },
            { searchDisabled: true },
            { searchDisabled: true },
            { searchDisabled: true },
            { name: "email" },
            { searchDisabled: true },
            { name: "recoveryEmail" },
            { searchDisabled: true },
            { searchDisabled: true },
            { searchDisabled: true },
            { name: "country" },
            { name: "status", options: gmailStatusSelections },
            { searchDisabled: true },
            { searchDisabled: true },
            { searchDisabled: true }
        ]);

        var datatableConfig = abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            lengthMenu: [20, 30, 50, 100, 200],
            searching: true,
            autoWidth: false,
            scrollCollapse: true,
            orderCellsTop: true,
            order: [[0, "desc"]],
            ajax: abp.libs.datatables.createAjax(gmailServer.controllers.gmail.getList, () => {
                return devmoba.datatables.searchHelper.getSearchConditions();
            }),
            columnDefs: [
                {
                    orderable: false,
                    targets: [1],
                    render: function (data, type, row, meta) {
                        if (data && type === 'display') {
                            let m = moment(data);
                            data = `<span>${m.local().format('YYYY/MM/DD')}</span>`;
                        }
                        return data;
                    }
                },
                {
                    orderable: false,
                    targets: [2, 3, 4, 5, 6, 8, 9, 10],
                },
                {
                    orderable: false,
                    targets: [7],
                    render: function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    targets: [11],
                    render: function (data, type, row, meta) {
                        var status = gmailStatusSelections.find(x => x.value == data);
                        return status.text;
                    }
                },
                {
                    targets: [12],
                    render: function (data, type, row, meta) {
                        if (data && type === 'display') {
                            let m = moment.utc(data);
                            data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD HH:mm')}</span>`;
                        }
                        return data;
                    }
                },
                {
                    targets: [13],
                    render: function (data, type, row, meta) {
                        if (data && type === 'display') {
                            let m = moment.utc(data);
                            data = `<span title="${m.fromNow()}">${m.local().format('YYYY/MM/DD HH:mm')}</span>`;
                        }
                        return data;
                    }
                },
                {
                    targets: [14],
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Delete'),
                                    //iconClass: "fas fa-trash-alt",
                                    visible: function (data) {
                                        return abp.auth.isGranted('GmailGroup.Delete');
                                    },
                                    confirmMessage: data => l('DeleteConfirm'),
                                    action: data => {
                                        gmailServer.controllers.gmail.delete(data.record.id).then(() => {
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
                { data: "id", width: "100px", class: "content-cell" },
                { data: "date", width: "150px", class: "content-cell" },
                { data: "firstName", width: "100px", class: "content-cell" },
                { data: "lastName", width: "100px", class: "content-cell" },
                { data: "email", width: "250px", class: "content-cell" },
                { data: "password", width: "200px", class: "content-cell" },
                { data: "recoveryEmail", width: "200px", class: "content-cell" },
                { data: "dateOfBirth", width: "120px", class: "content-cell" },
                { data: "gender", width: "80px", class: "content-cell" },
                { data: "timezone", width: "100px", class: "content-cell" },
                { data: "country", width: "100px", class: "content-cell" },
                { data: "status", width: "150px", class: "content-cell" },
                { data: "created", width: "130px", class: "content-cell" },
                { data: "updated", width: "130px", class: "content-cell" },
                { data: null, width: "100px", class: "content-cell" }
            ]
        });

        var dataTable = $('#gmailTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));
    });
});