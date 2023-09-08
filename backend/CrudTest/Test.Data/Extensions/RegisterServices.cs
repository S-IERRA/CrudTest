using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Data.Caching;
using Test.Data.Database;

namespace Test.Data.Extensions;

public static class RegisterServices
{
    public static void RegisterDataServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContextFactory<EntityFrameworkContext>(options =>
        {
            string test = configuration.GetConnectionString("Database");
            
            options.UseSqlServer(test);
            options.EnableSensitiveDataLogging();
        });

        serviceCollection.AddSingleton(new RedisConnectionManager(configuration.GetConnectionString("RedisConnection")));
    }
}