using Volo.Abp;
using Volo.Abp.Testing;

namespace Wafi.Abp.OpenAISemanticKernel;

public abstract class OpenAISemanticKernelTestBase : AbpIntegratedTest<OpenAISemanticKernelTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
