$(function () {
    var l = abp.localization.getResource('SmartHR');
    var createModal = new abp.ModalManager(abp.appPath + 'WorkSpaces/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'WorkSpaces/EditModal');

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
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function (data) {
                                    return l('WorkspaceDeletionConfirmationMessage', data.record.name);
                                },
                                action: function (data) {
                                    wafi.abp.workspaces.services.workspace
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

    $('#NewWorkspaceButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

}); 