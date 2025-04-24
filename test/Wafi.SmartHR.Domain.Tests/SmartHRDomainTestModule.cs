using Volo.Abp.Modularity;

namespace Wafi.SmartHR;

[DependsOn(
    typeof(SmartHRDomainModule),
    typeof(SmartHRTestBaseModule)
)]
public class SmartHRDomainTestModule : AbpModule
{

}
