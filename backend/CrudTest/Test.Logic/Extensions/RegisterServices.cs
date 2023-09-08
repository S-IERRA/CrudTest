using Hangfire;
using Hangfire.MemoryStorage;
using Test.Logic.Abstractions;
using Test.Logic.HangfireJobs;
using Test.Logic.Logic;

namespace Test.Logic.Extensions;

public static class RegisterServices
{
    public static void RegisterLogicServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHangfireServer();
        serviceCollection.AddHangfire(config =>
        {
            config.UseMemoryStorage();
        });
        
        serviceCollection.AddScoped(typeof(ICachedItemService), typeof(CachedItemImplementation));
        serviceCollection.AddScoped(typeof(IDbItemService), typeof(DbItemImplementation));

        serviceCollection.AddScoped(typeof(IItemService), typeof(ItemServiceImplementation));
    }
    
    public static void RegisterHangfireJobs(this IApplicationBuilder applicationBuilder)
    { 
        applicationBuilder.UseHangfireDashboard();

        RecurringJob.AddOrUpdate<RefreshCacheJob>("refresh-cache", x => x.RefreshCache(), Cron.Hourly);
    }
}