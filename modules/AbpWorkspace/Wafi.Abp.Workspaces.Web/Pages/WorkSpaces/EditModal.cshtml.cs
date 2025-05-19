using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Wafi.Abp.Workspaces.Services;
using Wafi.Abp.Workspaces.Services.Dtos;

namespace Wafi.Abp.Workspaces.Web.Pages.WorkSpaces;

public class EditModalModel : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public WorkspaceDto Workspace { get; set; }

    private readonly IWorkspaceAppService _workspaceAppService;

    public EditModalModel(IWorkspaceAppService workspaceAppService)
    {
        _workspaceAppService = workspaceAppService;
    }

    public async Task OnGetAsync()
    {
        Workspace = await _workspaceAppService.GetAsync(Id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _workspaceAppService.UpdateAsync(Id, Workspace.Name);
        return NoContent();
    }
} 