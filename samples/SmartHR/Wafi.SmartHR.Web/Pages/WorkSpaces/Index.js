$(function () {
    var l = abp.localization.getResource('SmartHR');
    var createModal = new abp.ModalManager(abp.appPath + 'Workspaces/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Workspaces/EditModal');

    var dataTable = $('#WorkspacesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(wafi.abp.workspaces.services.workspace.getAll),
            columnDefs: [
                {
                    title: l('Name'),
                    data: 'name'
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('SmartHR.Workspaces.Edit'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('SmartHR.Workspaces.Delete'),
                                confirmMessage: function (data) {
                                    return l('WorkspaceDeletionConfirmationMessage', data.record.firstName + ' ' + data.record.lastName);
                                },
                                action: function (data) {
                                    wafi.smartHR.workspaces.workspace
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

    $('#NewWorkspaceButton').on('click', function () {
        e.preventDefault();
        createModal.open();
    });

}); 