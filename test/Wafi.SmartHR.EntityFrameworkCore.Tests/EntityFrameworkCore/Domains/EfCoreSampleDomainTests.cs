using Wafi.SmartHR.Samples;
using Xunit;

namespace Wafi.SmartHR.EntityFrameworkCore.Domains;

[Collection(SmartHRTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<SmartHREntityFrameworkCoreTestModule>
{

}
