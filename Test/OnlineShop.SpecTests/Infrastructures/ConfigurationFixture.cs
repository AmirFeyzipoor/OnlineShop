using Microsoft.Extensions.Configuration;

namespace OnlineShop.SpecTests.Infrastructures
{
    public class ConfigurationFixture
    {
        public TestSettings Value { get; private set; }

        public ConfigurationFixture()
        {
            Value = GetSettings();
        }

        private TestSettings GetSettings()
        {
            var configurations = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                "appsettings.json", 
                optional: true, 
                reloadOnChange: false)
                .Build();

            var settings = new TestSettings();
            configurations.Bind("ConnectionStrings",settings);
            return settings;
        }
    }
}
