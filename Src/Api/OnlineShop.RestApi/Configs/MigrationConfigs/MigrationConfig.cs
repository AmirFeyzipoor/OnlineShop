using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using OnlineShop.Migrations;
using OnlineShop.ReadableDataMigrations;
using OnlineShop.RestApi.Configs.ServiceConfigs.ServicesPrerequisites;

namespace OnlineShop.RestApi.Configs.MigrationConfigs;

public static class MigrationConfig
{
    private static readonly ConnectionStrings _dbConnectionStrings = new();

    private static void Initialized(WebApplicationBuilder builder)
    {
        builder.Configuration.Bind("ConnectionStrings", _dbConnectionStrings);
    }

    public static void UpdateDataBases(this WebApplicationBuilder builder)
    {
        Initialized(builder);

        if (!Directory.EnumerateFileSystemEntries(@"\OnlineShop\Src\Migration\OnlineShop.Migrations\Migrations").Any()) return;
        
        var connectionStrings = new List<string>
        {
            _dbConnectionStrings.WritableDb,
            _dbConnectionStrings.WritableDbTest
        };
        foreach (var connectionString in connectionStrings)
        {
            using var scope = ConfigureMigration(connectionString).CreateScope();

            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
    
    public static void UpdateReadableDataBases(this WebApplicationBuilder builder)
    {
        Initialized(builder);

        if (!Directory.EnumerateFileSystemEntries(@"\OnlineShop\Src\Migration\OnlineShop.ReadableDataMigrations\Migrations").Any()) return;
        
        var connectionStrings = new List<string>
        {
            _dbConnectionStrings.ReadableDb,
            _dbConnectionStrings.ReadableDbTest
        };
        foreach (var connectionString in connectionStrings)
        {
            using var scope = ConfigureMigrationForReadableDataMigrations(connectionString).CreateScope();

            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }

    private static IServiceProvider ConfigureMigration(string connectionString)
    {
        CreateDatabase(connectionString);

        var runner = CreateRunner(connectionString);

        return runner;
    }
    
    private static IServiceProvider ConfigureMigrationForReadableDataMigrations(string connectionString)
    {
        CreateDatabase(connectionString);

        var runner = CreateRunnerForReadableDataMigrations(connectionString);

        return runner;
    }

    private static void CreateDatabase(string connectionString)
    {
        var databaseName = GetDatabaseName(connectionString);
        var masterConnectionString = ChangeDatabaseName(
            connectionString,
            "master");
        var commandScript = $"if db_id(N'{databaseName}') is null " +
                            $"create database [{databaseName}]";

        using var connection = new SqlConnection(masterConnectionString);
        using var command = new SqlCommand(commandScript, connection);
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    private static string ChangeDatabaseName(
        string connectionString,
        string databaseName)
    {
        var csb = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = databaseName
        };
        return csb.ConnectionString;
    }

    private static string GetDatabaseName(string connectionString)
    {
        return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
    }

    private static IServiceProvider CreateRunner(
        string connectionString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(_ => _
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(ScriptResourceManager).Assembly).For.All())
            .AddSingleton<ScriptResourceManager>()
            .AddLogging(_ => _.AddFluentMigratorConsole())
            .BuildServiceProvider();
    }
    
    private static IServiceProvider CreateRunnerForReadableDataMigrations(
        string connectionString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(_ => _
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(ReadableScriptResourceManager).Assembly).For.All())
            .AddSingleton<ReadableScriptResourceManager>()
            .AddLogging(_ => _.AddFluentMigratorConsole())
            .BuildServiceProvider();
    }
}