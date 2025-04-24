using Volo.Abp.Modularity;

namespace Wafi.SmartHR;

public abstract class SmartHRApplicationTestBase<TStartupModule> : SmartHRTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
