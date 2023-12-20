namespace OnlineShop.SpecTests.Infrastructures;

[CollectionDefinition(
    nameof(ConfigurationFixture), 
    DisableParallelization = false)]
public class ConfigurationCollectionFixture : 
    ICollectionFixture<ConfigurationFixture>
{
}