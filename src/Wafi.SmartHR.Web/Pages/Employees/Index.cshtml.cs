using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.Employees.Dtos;

namespace Wafi.SmartHR.Web.Pages.Employees
{
    public class IndexModel : SmartHRPageModel
    {
        private readonly IEmployeeAppService _employeeAppService;

        public IndexModel(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }

        public async Task OnGetAsync()
        {
            await Task.CompletedTask;
        }
    }
} 