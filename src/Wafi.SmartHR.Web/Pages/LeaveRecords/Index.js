$(function () {
    var l = abp.localization.getResource('WafiSmartHR');
    var createModal = new abp.ModalManager(abp.appPath + 'LeaveRecords/CreateModal');
    var updateStatusModal = new abp.ModalManager(abp.appPath + 'LeaveRecords/UpdateStatusModal');

    var dataTable = $('#LeaveRecordsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "desc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(wafi.smartHR.leaveRecords.leaveRecord.getList),
            columnDefs: [
                {
                    title: l('Employee'),
                    data: "employeeName"
                },
                {
                    title: l('StartDate'),
                    data: "startDate",
                    render: function (data) {
                        return luxon.DateTime.fromISO(data).toLocaleString();
                    }
                },
                {
                    title: l('EndDate'),
                    data: "endDate",
                    render: function (data) {
                        return luxon.DateTime.fromISO(data).toLocaleString();
                    }
                },
                {
                    title: l('TotalDays'),
                    data: "totalDays"
                },
                {
                    title: l('Status'),
                    data: "status",
                    render: function (data) {
                        return l('Enum:LeaveStatus:' + data);
                    }
                },
                {
                    title: l('Reason'),
                    data: "reason"
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('UpdateStatus'),
                                    action: function (data) {
                                        updateStatusModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    confirmMessage: function (data) {
                                        return l('LeaveRecordDeletionConfirmationMessage');
                                    },
                                    action: function (data) {
                                        wafi.smartHR.leaveRecords.leaveRecord
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.info(l('SuccessfullyDeleted'));
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                }
            ]
        })
    );

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    updateStatusModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewLeaveRecordButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
}); 