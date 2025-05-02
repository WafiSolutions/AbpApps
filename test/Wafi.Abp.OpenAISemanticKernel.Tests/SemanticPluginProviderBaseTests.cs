using Shouldly;
using Xunit;

namespace Wafi.Abp.OpenAISemanticKernel;

public class SemanticPluginProviderBaseTests
{
    [Fact]
    public void Should_Create_Plugin_With_Correct_Name_And_Instance()
    {
        // Arrange
        var testPlugin = new TestPlugin();
        var provider = new TestPluginProvider(testPlugin);

        // Act
        var plugin = provider.GetPlugin();

        // Assert
        plugin.ShouldNotBeNull();
        plugin.Name.ShouldBe("TestPlugin");
        plugin.Instance.ShouldBe(testPlugin);
    }

    [Fact]
    public void Should_Use_Custom_Name_When_Provided()
    {
        // Arrange
        var testPlugin = new TestPlugin();
        var customName = "CustomPluginName";
        var provider = new TestPluginProviderWithCustomName(testPlugin, customName);

        // Act
        var plugin = provider.GetPlugin();

        // Assert
        plugin.ShouldNotBeNull();
        plugin.Name.ShouldBe(customName);
        plugin.Instance.ShouldBe(testPlugin);
    }

    // Test classes
    private class TestPlugin
    {
        public string SayHello() => "Hello";
    }

    private class TestPluginProvider : SemanticPluginProviderBase<TestPlugin>
    {
        public TestPluginProvider(TestPlugin pluginInstance) : base(pluginInstance)
        {
        }
    }

    private class TestPluginProviderWithCustomName : SemanticPluginProviderBase<TestPlugin>
    {
        private readonly string _customName;

        public TestPluginProviderWithCustomName(TestPlugin pluginInstance, string customName)
            : base(pluginInstance)
        {
            _customName = customName;
        }

        public override string Name => _customName;
    }
}
