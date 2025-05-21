<h1 style="display: flex; align-items: center;">
  <img src="https://img.icons8.com/fluency/48/briefcase.png" alt="Briefcase Icon" style="vertical-align: middle; margin-right: 10px;">
  Multi Workspace Management for ABP Applications
</h1>

In this article, we show how you can separate your application's resources by workspaces—just like you see in platforms such as GitHub, Bitbucket, or Trello, where you can create different repositories or boards under different workspaces. 

We implemented workspace management in a generic and reusable way, inspired by how ABP implements tenant management. Just as ABP automatically filters and separates data for each tenant, our approach does the same for workspaces—making it easy to keep data isolated and organized.

## ✅ Demo 

In this sample, we created three offices as workspaces:
- Wafi Solutions BD
- Wafi Solutions UK
- Wafi Solutions KSA

We then created employees under each office. This implementation demonstrates how easily you can manage and separate resources (like employees) under different workspaces. 

![Demo](/etc/img/workspace.gif)

After completing these steps, your ABP application will have a fully functioning workspace management system that:

1. Isolates data between workspaces
2. Provides workspace management UI
3. Handles workspace-based routing
4. Works seamlessly with ABP's existing features

Users can:
- Create new workspaces
- Switch between workspaces
- Manage employees under workspace. 

## 🏗️ Architecture Overview

The solution consists of two primary modules:

1. **`Wafi.Abp.Workspaces.Core`**
   Core module that provides the fundamental workspace management capabilities:
   - Workspace entity definitions and interfaces
   - Middleware for workspace resolution from HTTP headers
   - EF Core integration through custom DbContext
   - Data filtering based on current workspace

2. **`Wafi.Abp.Workspaces.Web`**
   UI components and controllers for workspace management in ABP web applications:
   - Workspace selector UI components
   - HTTP interceptors for automatic workspace header injection
   - Management interface for workspace configuration

## 🔧 How It Works

The workspace system implements data isolation within your company similar to ABP's multi-tenancy, but at a more granular level - enabling teams, departments, or projects to have their own isolated workspaces within the same tenant.

### Core Components and Their Interactions

1. **Entity Integration with `IWorkspace.cs`**: 
   This simple interface marks entities that should be segregated by workspace. Any entity implementing `IWorkspace` will automatically be filtered by the current workspace context.

```csharp
// IWorkspace interface definition
public interface IWorkspace
{
    Guid? WorkspaceId { get; set; }
}
```

2. **HTTP Request Processing with `WorkspaceResolutionMiddleware.cs`**: 
   This ASP.NET Core middleware intercepts all incoming requests to determine the current workspace context. It works in the request pipeline to:
   - Extract workspace information from HTTP headers
   - Set the current workspace for the duration of the request
   - Apply workspace context to all downstream operations

```csharp
// Key part of the middleware
public async Task InvokeAsync(HttpContext context, RequestDelegate next)
{
    var workspaceResolveContext = new WorkspaceResolveContext(context);

    // Try resolving workspace from various sources
    foreach (var workspaceResolver in _options.WorkspaceResolvers)
    {
        await workspaceResolver.ResolveAsync(workspaceResolveContext);
        if (workspaceResolveContext.WorkspaceId.HasValue) break;
    }

    // Apply workspace context for the request duration
    if (workspaceResolveContext.WorkspaceId.HasValue)
    {
        using (_currentWorkspace.Change(workspaceResolveContext.WorkspaceId.Value))
        {
            await next(context);
        }
    }
    else
    {
        await next(context);
    }
}
```

3. **Header-Based Resolution with `WorkspaceIdHeaderResolveContributor.cs`**: 
   This component extracts the workspace ID from the HTTP header (`X-Workspace-Id`), enabling seamless workspace resolution for API requests.

```csharp
// Workspace ID Header Resolver
public class WorkspaceIdHeaderResolveContributor : IWorkspaceResolveContributor, ITransientDependency
{
    public const string HeaderName = "X-Workspace-Id";
    public const string ContributorName = "WorkspaceIdHeader";

    public Task ResolveAsync(IWorkspaceResolveContext context)
    {
        var httpContext = context.GetHttpContext();
        if (httpContext == null) return Task.CompletedTask;

        var workspaceIdHeader = httpContext.Request.Headers[HeaderName];
        if (workspaceIdHeader.Count == 0 || string.IsNullOrWhiteSpace(workspaceIdHeader[0]))
            return Task.CompletedTask;

        if (Guid.TryParse(workspaceIdHeader[0], out var workspaceId))
        {
            context.WorkspaceId = workspaceId;
        }
        
        return Task.CompletedTask;
    }
}
```

4. **Data Filtering with `WorkspaceDbContextBase.cs`**: 
   This specialized DbContext provides automatic data filtering for all workspace-aware entities by:
   - Automatically applying the current workspace ID to new entities
   - Preventing accidental workspace ID modifications
   - Filtering query results to only show data from the current workspace

```csharp
// Simplified version of WorkspaceDbContextBase filter expression
protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(ModelBuilder modelBuilder)
{
    // Get base filter from ABP
    var baseExpression = base.CreateFilterExpression<TEntity>(modelBuilder);
    
    // Skip if entity doesn't implement IWorkspace
    if (!typeof(IWorkspace).IsAssignableFrom(typeof(TEntity)))
    {
        return baseExpression;
    }

    // Find the workspace ID property
    var prop = modelBuilder.Entity<TEntity>()
        .Metadata.FindProperty(nameof(IWorkspace.WorkspaceId))!;
    
    var columnName = prop.GetColumnName() ?? prop.Name;

    // Create workspace filter: only show entities from current workspace
    Expression<Func<TEntity, bool>> workspaceFilter = e =>
        !IsMultiWorkspaceFilterEnabled
        || CurrentWorkspace.Id == null
        || EF.Property<Guid?>(e, columnName) == CurrentWorkspace.Id;

    // Combine with existing filters
    if (baseExpression == null) return workspaceFilter;
    return QueryFilterExpressionHelper.CombineExpressions(baseExpression, workspaceFilter);
}
```

5. **Client-Side Integration with `http-interceptor.js`**: 
   This JavaScript component automatically includes the current workspace ID in all AJAX requests, ensuring seamless workspace context propagation from UI to API calls.

```javascript
// Automatically included from http-interceptor.js
function getWorkspaceId() {
    return localStorage.getItem('selectedWorkspaceId');
}

jQuery(document).ajaxSend(function (event, xhr, settings) {
    const workspaceId = getWorkspaceId();
    if (workspaceId) {
        xhr.setRequestHeader('X-Workspace-Id', workspaceId);
    }
});
```

The system integrates seamlessly with ABP's existing multi-tenancy, allowing:
- Multi-tenant applications with workspace isolation within each tenant
- Single-tenant applications with departmental/team separation
- Combinations where some data is tenant-specific and some is workspace-specific


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

Use the `IWorkspace` interface in your entities where you want to enable the workspace feature. This is very similar to IMultitenant, making integration straightforward:

```csharp
// IWorkspace interface definition
public interface IWorkspace
{
    Guid? WorkspaceId { get; set; }
}

// Example implementation
public class YourEntity : FullAuditedAggregateRoot<Guid>, IMultiTenant, IWorkspace
{
    public Guid? TenantId { get; set; }
    public Guid? WorkspaceId { get; set; }
    
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

The `WorkspaceDbContextBase` handles:
- Applying the current workspace ID to new entities
- Preventing workspace ID modifications in existing entities
- Filtering queries based on the current workspace context

```csharp
// Example of SaveChanges method in WorkspaceDbContextBase
public override int SaveChanges(bool acceptAllChangesOnSuccess)
{
    ApplyCurrentWorkspaceId();
    return base.SaveChanges(acceptAllChangesOnSuccess);
}

private void ApplyCurrentWorkspaceId()
{
    if (CurrentWorkspace?.Id == null) return;

    var currentWorkspaceId = CurrentWorkspace.Id.Value;

    foreach (var entry in ChangeTracker.Entries()
        .Where(e =>
            e.Entity is IWorkspace &&
            (e.State == EntityState.Added || e.State == EntityState.Modified)))
    {
        // Stamp the FK column via EF Core API
        entry.Property(nameof(IWorkspace.WorkspaceId)).CurrentValue = currentWorkspaceId;

        if (entry.State == EntityState.Modified)
        {
            // Prevent accidental overwrites
            entry.Property(nameof(IWorkspace.WorkspaceId)).IsModified = false;
        }
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

## 📚 Additional Resources

For a deeper understanding of the module's implementation, architecture decisions, and advanced customization options, see:

- [ABP Framework Documentation](https://docs.abp.io/en/abp/latest/)
- [ABP Multi-Tenancy Documentation](https://docs.abp.io/en/abp/latest/Multi-Tenancy)
- [Full Module Source Code](https://github.com/WafiSolutions/AbpApps/tree/main/modules/AbpWorkspace) 
