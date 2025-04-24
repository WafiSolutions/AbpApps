using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Wafi.SmartHR.Pages;

[Collection(SmartHRTestConsts.CollectionDefinitionName)]
public class Index_Tests : SmartHRWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
