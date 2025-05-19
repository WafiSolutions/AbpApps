namespace Wafi.SmartHR.Permissions;

public static class SmartHRPermissions
{
    public const string GroupName = "SmartHR";

    public static class Employees
    {
        public const string Default = GroupName + ".Employees";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class LeaveRecords
    {
        public const string Default = GroupName + ".LeaveRecords";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string UpdateStatus = Default + ".UpdateStatus";
    }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
}
