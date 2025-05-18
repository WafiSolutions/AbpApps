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
    // your existing dependencies
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
- Change the base class of your DbContext from `AbpDbContext` to `WorkspaceDbContextBase`
- Add `builder.ConfigureWorkspaces()` in `OnModelCreating` method

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
        
        // Apply workspace filter to relevant entities
        builder.ConfigureWorkspaces();
        
        // Your existing model configuration
    }
}
```

---

### 🔹 Step 3: Set Up Workspace Management UI

The Workspaces Web module automatically adds a Workspace Management page to your ABP application. 
The navigation to the workspace management and switching is implemented in `WorkspaceSelectorViewComponent`,
which we have included in the user menu toolbar.

1. Add the component to your layout:

```csharp
@await Component.InvokeAsync(typeof(WorkspaceSelectorViewComponent))
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