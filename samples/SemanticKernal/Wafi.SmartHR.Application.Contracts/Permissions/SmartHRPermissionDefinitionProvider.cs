using Wafi.SmartHR.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Wafi.SmartHR.Permissions;

public class SmartHRPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SmartHRPermissions.GroupName);

        // Employees permissions
        var employeesPermission = myGroup.AddPermission(SmartHRPermissions.Employees.Default, L("Permission:Employees"));
        employeesPermission.AddChild(SmartHRPermissions.Employees.Create, L("Permission:Employees.Create"));
        employeesPermission.AddChild(SmartHRPermissions.Employees.Edit, L("Permission:Employees.Edit"));
        employeesPermission.AddChild(SmartHRPermissions.Employees.Delete, L("Permission:Employees.Delete"));

        // LeaveRecords permissions
        var leaveRecordsPermission = myGroup.AddPermission(SmartHRPermissions.LeaveRecords.Default, L("Permission:LeaveRecords"));
        leaveRecordsPermission.AddChild(SmartHRPermissions.LeaveRecords.Create, L("Permission:LeaveRecords.Create"));
        leaveRecordsPermission.AddChild(SmartHRPermissions.LeaveRecords.Edit, L("Permission:LeaveRecords.Edit"));
        leaveRecordsPermission.AddChild(SmartHRPermissions.LeaveRecords.Delete, L("Permission:LeaveRecords.Delete"));
        leaveRecordsPermission.AddChild(SmartHRPermissions.LeaveRecords.UpdateStatus, L("Permission:LeaveRecords.UpdateStatus"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SmartHRResource>(name);
    }
}
