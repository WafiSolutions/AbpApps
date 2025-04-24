$(function () {
    var l = abp.localization.getResource('WafiSmartHR');
    var createModal = new abp.ModalManager(abp.appPath + 'Employees/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Employees/EditModal');

    var dataTable = $('#EmployeesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: false,
            paging: true,
            order: [[0, "asc"]],
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(wafi.smartHR.employees.employee.getList),
            columnDefs: [
                {
                    title: l('Name'),
                    data: null,
                    render: function (data) {
                        return data.firstName + ' ' + data.lastName;
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
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
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