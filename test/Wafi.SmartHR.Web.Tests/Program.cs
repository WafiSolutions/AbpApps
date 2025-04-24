using Microsoft.AspNetCore.Builder;
using Wafi.SmartHR;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Wafi.SmartHR.Web.csproj"); 
await builder.RunAbpModuleAsync<SmartHRWebTestModule>(applicationName: "Wafi.SmartHR.Web");

public partial class Program
{
}
