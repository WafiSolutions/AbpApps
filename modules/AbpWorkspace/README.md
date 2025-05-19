<h1 style="display: flex; align-items: center;">
  <img src="https://img.icons8.com/fluency/48/briefcase.png" alt="Briefcase Icon" style="vertical-align: middle; margin-right: 10px;">
  Multi Workspace Management for ABP Applications
</h1>

This guide explains how to integrate **Wafi.Abp.Workspaces** with the **ABP.io** framework. It offers a comprehensive workspace management solution for both multi-tenant and single-tenant ABP applications, allowing organizations to create isolated workspaces with distinct configurations, users, and data.


## 🏗️ Architecture Overview

The solution consists of two primary modules:

1. **`Wafi.Abp.Workspaces.Core`**
   Core module that provides the fundamental workspace management capabilities.

2. **`Wafi.Abp.Workspaces.Web`**
   UI components and controllers for workspace management in ABP web applications.


## ⚙️ Implementation Steps

Follow these steps to integrate Workspaces into your ABP application.

---

### 🔹 Step 1: Add the Workspace in your Entities

1. Add the Workspace packages in domain layer:

```bash
dotnet add package Wafi.Abp.Workspaces.Core
```

2. Add the module dependencies in your modules:

```csharp
[DependsOn(
    typeof(WafiAbpWorkspaceModule)
)]
public class YourAppDomainModule : AbpModule
{
    // ...
}
```

3. Make Your Entities Workspace-Aware

Use the `IWorkspace` interfaces in your entities where you want to enable the workspace feature and implement the interface. This is very similar to IMultitenant, making integration straightforward:

```csharp
public class YourEntity : FullAuditedAggregateRoot<Guid>, IMultiTenant, IWorkspaceEntity
{
    public Guid? TenantId { get; set; }
    public string WorkspaceId { get; set; }
    
    // Your existing entity properties
}
```


### 🔹 Step 2: Configure Your Entity Framework Core Context

1. Change the base class of your `DbContext` from `AbpDbContext` to `WorkspaceDbContextBase`.
2. Add `builder.ConfigureWorkspaces();` in the `OnModelCreating` method to register workspace-related entities and configurations.
3. **Run a new migration and update the database** to apply the necessary schema changes.

```csharp
public class YourAppDbContext : WorkspaceDbContextBase<YourAppDbContext>
{
    public YourAppDbContext(DbContextOptions<YourAppDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Register workspace-related entity mappings and configurations
        builder.ConfigureWorkspaces();

        // Your existing model configuration
    }
}
```
---

### 🔹 Step 3: Set Up Workspace Management UI

1. Add the Workspace packages in web layer:

```bash
dotnet add package Wafi.Abp.Workspaces.Web
```

2. Add the module dependencies in your modules:

```csharp
[DependsOn(
    typeof(WorkspacesWebModule)
)]
public class YourWebModule : AbpModule
{
    // ...
}
```
3. Add the Workspace Selector in the leptonx topbar container:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    public class YourToolbarContributor : IToolbarContributor
    {
        public async Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name == LeptonXLiteToolbars.Main)
            {
                if (context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
                {
                    context.Toolbar.Items.AddFirst(new ToolbarItem(typeof(WorkspaceSelectorViewComponent)));
                }
            }
        }
    }
}
```

> The workspace selector will now appear in your application's topbar, providing:
> - A dropdown to switch between workspaces
> - Visual indication of the current workspace
> - Quick access to workspace management
> - Seamless integration with ABP's LeptonXLite theme

---
 
## ✅ Final Outcome 

![Demo](/etc/img/workspace.gif)

After completing these steps, your ABP application will have a fully functioning workspace management system that:

1. Isolates data between workspaces
2. Provides workspace management UI
3. Handles workspace-based routing
4. Works seamlessly with ABP's existing features

Users can:
- Create new workspaces
- Switch between workspaces
- Manage workspace settings and users

---

## 📚 Additional Resources

For a deeper understanding of the module's implementation, architecture decisions, and advanced customization options, see:

- [ABP Framework Documentation](https://docs.abp.io/en/abp/latest/)
- [ABP Multi-Tenancy Documentation](https://docs.abp.io/en/abp/latest/Multi-Tenancy)
- [Full Module Source Code](https://github.com/WafiSolutions/AbpApps/tree/main/modules/AbpWorkspace) 
