using Volo.Abp.Settings;

namespace Wafi.SmartHR.Settings;

public class SmartHRSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SmartHRSettings.MySetting1));
    }
}
