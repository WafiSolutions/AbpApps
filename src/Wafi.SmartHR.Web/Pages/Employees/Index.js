$(function () {
    var l = abp.localization.getResource('SmartHR');
    var createModal = new abp.ModalManager(abp.appPath + 'Employees/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Employees/EditModal');

    var dataTable = $('#EmployeesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(wafi.smartHR.employees.employee.getPagedList),
            columnDefs: [
                {
                    title: l('Name'),
                    data: 'firstName',
                    render: function (data, type, row) {
                        return row.firstName + ' ' + row.lastName;
                    }
                },
                {
                    title: l('Email'),
                    data: "email"
                },
                {
                    title: l('PhoneNumber'),
                    data: "phoneNumber"
                },
                {
                    title: l('DateOfBirth'),
                    data: "dateOfBirth",
                    render: function (data) {
                        return luxon.DateTime.fromISO(data).toLocaleString();
                    }
                },
                {
                    title: l('JoiningDate'),
                    data: "joiningDate",
                    render: function (data) {
                        return luxon.DateTime.fromISO(data).toLocaleString();
                    }
                },
                {
                    title: l('TotalLeaveDays'),
                    data: "totalLeaveDays"
                },
                {
                    title: l('RemainingLeaveDays'),
                    data: "remainingLeaveDays"
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('SmartHR.Employees.Edit'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('SmartHR.Employees.Delete'),
                                confirmMessage: function (data) {
                                    return l('EmployeeDeletionConfirmationMessage', data.record.firstName + ' ' + data.record.lastName);
                                },
                                action: function (data) {
                                    wafi.smartHR.employees.employee
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

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewEmployeeButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
}); 