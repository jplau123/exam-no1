using examNo1.domain.Clients;
using examNo1.infrastructure.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace examNo1.infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IExchangeRateApiClient, ExchangeRateApiClient>();

        services.AddHttpClient();
        //services.AddHostedService<DbCleanupWorkerService>();

        //string dbConnectonString = config.GetConnectionString("pgConnection")
        //        ?? throw new ConfigException("Connection string cannot be found.");

        //services.AddScoped<IDbConnection>((serviceProvider) => new NpgsqlConnection(dbConnectonString));
        //// DbUp
        //EnsureDatabase.For.PostgresqlDatabase(dbConnectonString);
        //var upgrader = DeployChanges.To
        //        .PostgresqlDatabase(dbConnectonString)
        //        .WithScriptsEmbeddedInAssembly(typeof(ExchangeRateApiClient).Assembly)
        //        .LogToNowhere()
        //        .Build();

        //var result = upgrader.PerformUpgrade();
    }
}
