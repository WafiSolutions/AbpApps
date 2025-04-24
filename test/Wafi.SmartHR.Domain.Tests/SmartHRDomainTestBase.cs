using Volo.Abp.Modularity;

namespace Wafi.SmartHR;

/* Inherit from this class for your domain layer tests. */
public abstract class SmartHRDomainTestBase<TStartupModule> : SmartHRTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
