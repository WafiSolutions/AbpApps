namespace Wafi.Abp.Workspace.Core;

public static class SmartHRDbProperties
{
    public static string DbTablePrefix { get; set; } = "Workspace";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Default";
}
