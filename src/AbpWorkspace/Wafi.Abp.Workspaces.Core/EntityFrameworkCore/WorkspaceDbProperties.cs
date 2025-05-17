namespace Wafi.Abp.Workspaces.EntityFrameworkCore;

public static class WorkspaceDbProperties
{
    public static string DbTablePrefix { get; set; } = "Workspace";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Default";
}
