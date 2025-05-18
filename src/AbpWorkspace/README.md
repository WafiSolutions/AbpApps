<h1 style="display: flex; align-items: center;">
  <img src="https://img.icons8.com/color/48/workspace.png" alt="Workspace Logo" style="vertical-align: middle; margin-right: 10px;">
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

### 🔹 Step 1: Add the Package to Your Projects

1. Add the Workspace packages:

```bash
dotnet add YourApp.Domain.csproj package Wafi.Abp.Workspaces.Core
dotnet add YourApp.Web.csproj package Wafi.Abp.Workspaces.Web
```

2. Add the module dependencies in your modules:

```csharp
[DependsOn(
    // your existing dependencies
    typeof(WafiAbpWorkspacesCoreModule)
)]
public class YourAppDomainModule : AbpModule
{
    // ...
}

[DependsOn(
    // your existing dependencies
    typeof(WafiAbpWorkspacesWebModule)
)]
public class YourAppWebModule : AbpModule
{
    // ...
}
```

3. Add the following configuration to your appsettings.json:

```json
{
  "Workspaces": {
    "DefaultWorkspaceName": "Default",
    "EnableMultiWorkspaces": true,
    "WorkspaceRoutingStrategy": "Subdomain"
  }
}
```

---

### 🔹 Step 2: Configure Your Entity Framework Core Context

Update your DbContext to support workspaces by implementing the workspace filter:

```csharp
public class YourAppDbContext : AbpDbContext<YourAppDbContext>, IWorkspaceFilterEnabled
{
    public YourAppDbContext(DbContextOptions<YourAppDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply workspace filter to relevant entities
        builder.ConfigureWorkspaces();
        
        // Your existing model configuration
    }
}
```

---

### 🔹 Step 3: Make Your Entities Workspace-Aware

Add the `IWorkspaceEntity` interface to any entity that should be isolated within a workspace:

```csharp
public class YourEntity : FullAuditedAggregateRoot<Guid>, IWorkspaceEntity
{
    public string WorkspaceId { get; set; }
    
    // Your existing entity properties
    public string Name { get; set; }
    // ...
}
```

---

### 🔹 Step 4: Set Up Workspace Management UI

The Workspaces Web module automatically adds a Workspace Management page to your ABP application. To customize the UI:

1. Override the default templates:

```csharp
Configure<AbpWorkspacesUiOptions>(options =>
{
    options.WorkspaceCreationTemplate = "~/Pages/Workspaces/CustomWorkspaceCreation.cshtml";
    options.WorkspaceSwitcherTemplate = "~/Pages/Workspaces/CustomWorkspaceSwitcher.cshtml";
});
```

2. Add workspace-related permissions to your permission definition:

```csharp
public static class YourAppPermissions
{
    public static class Workspaces
    {
        public const string GroupName = "Workspaces";
        public const string Create = GroupName + ".Create";
        public const string Edit = GroupName + ".Edit";
        public const string Delete = GroupName + ".Delete";
    }
}
```

---

### 🔹 Step 5: Use the Workspace Context in Your Services

1. Inject `IWorkspaceContext` to access the current workspace:

```csharp
public class YourService : ApplicationService
{
    private readonly IWorkspaceContext _workspaceContext;
    
    public YourService(IWorkspaceContext workspaceContext)
    {
        _workspaceContext = workspaceContext;
    }
    
    public async Task<string> GetCurrentWorkspaceAsync()
    {
        return _workspaceContext.Current.WorkspaceId;
    }
}
```

2. Switch workspaces programmatically when needed:

```csharp
public class WorkspaceSwitchService : ApplicationService
{
    private readonly IWorkspaceManager _workspaceManager;
    
    public WorkspaceSwitchService(IWorkspaceManager workspaceManager)
    {
        _workspaceManager = workspaceManager;
    }
    
    public async Task SwitchToWorkspaceAsync(string workspaceId)
    {
        await _workspaceManager.SetCurrentWorkspaceAsync(workspaceId);
    }
}
```

---

## 🚀 Advanced Features

### Multi-Tenant Support

The Workspace module seamlessly integrates with ABP's multi-tenancy:

```csharp
public class YourEntity : FullAuditedAggregateRoot<Guid>, IMultiTenant, IWorkspaceEntity
{
    public Guid? TenantId { get; set; }
    public string WorkspaceId { get; set; }
    
    // Your existing entity properties
}
```

### Workspace Routing Strategies

Choose from different routing strategies:

1. **Subdomain**: Access workspaces via `workspace-name.yourdomain.com`
2. **Path**: Access workspaces via `yourdomain.com/workspace-name`
3. **QueryString**: Access workspaces via `yourdomain.com?workspace=workspace-name`

Configure in your appsettings.json:

```json
{
  "Workspaces": {
    "WorkspaceRoutingStrategy": "Subdomain"
  }
}
```

---

## ✅ Final Outcome 

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
- [Full Module Source Code](https://github.com/WafiSolutions/Wafi.Abp.Workspaces) 