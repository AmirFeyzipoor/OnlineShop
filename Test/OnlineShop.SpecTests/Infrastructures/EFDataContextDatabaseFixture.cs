using OnlineShop.Infrastructure.ReadableData;
using OnlineShop.Infrastructure.WritableData;

namespace OnlineShop.SpecTests.Infrastructures
{
    [Collection(nameof(ConfigurationFixture))]
    public class EFDataContextDatabaseFixture : DatabaseFixture
    {
        public readonly ConfigurationFixture _configuration;

        public EFDataContextDatabaseFixture(ConfigurationFixture configuration)
        {
            _configuration = configuration;
        }

        public WritableDb CreateDataContext()
        {
            return new WritableDb(_configuration.Value.WritableDb);
        }
        
        public ReadableDb CreateReadDataContext()
        {
            return new ReadableDb(_configuration.Value.ReadableDb);
        }
    }
}
