$(function () {
    var l = abp.localization.getResource('SmartHR');
    var createModal = new abp.ModalManager(abp.appPath + 'LeaveRecords/CreateModal');
    var updateStatusModal = new abp.ModalManager(abp.appPath + 'LeaveRecords/UpdateStatusModal');

    var dataTable = $('#LeaveRecordsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: false,
            paging: true,
            order: [[1, "desc"]],
            searching: true,
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
                        items: [
                            {
                                text: l('UpdateStatus'),
                                visible: abp.auth.isGranted('SmartHR.LeaveRecords.UpdateStatus'),
                                action: function (data) {
                                    updateStatusModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('SmartHR.LeaveRecords.Delete'),
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