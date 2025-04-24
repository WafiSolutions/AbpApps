using Wafi.SmartHR.Samples;
using Xunit;

namespace Wafi.SmartHR.EntityFrameworkCore.Applications;

[Collection(SmartHRTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<SmartHREntityFrameworkCoreTestModule>
{

}
