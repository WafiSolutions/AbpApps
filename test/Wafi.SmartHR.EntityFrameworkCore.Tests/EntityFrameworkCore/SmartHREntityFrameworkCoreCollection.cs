using Xunit;

namespace Wafi.SmartHR.EntityFrameworkCore;

[CollectionDefinition(SmartHRTestConsts.CollectionDefinitionName)]
public class SmartHREntityFrameworkCoreCollection : ICollectionFixture<SmartHREntityFrameworkCoreFixture>
{

}
