using Volo.Abp.Modularity;

namespace Wafi.SmartHR;

[DependsOn(
    typeof(SmartHRApplicationModule),
    typeof(SmartHRDomainTestModule)
)]
public class SmartHRApplicationTestModule : AbpModule
{

}
