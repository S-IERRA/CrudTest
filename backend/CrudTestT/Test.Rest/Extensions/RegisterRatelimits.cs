using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Test.Rest.Extensions;

public static class RestRegisterExtensions
{
    public static IServiceCollection RegisterRestServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(options => {
            options.AddPolicy("CORSPolicy", corsPolicyBuilder => 
                corsPolicyBuilder.WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        serviceCollection.AddRateLimiter(_ => _
            .AddSlidingWindowLimiter("Api", options =>
            {
                options.PermitLimit = 10;
                options.Window = TimeSpan.FromSeconds(30);
                options.SegmentsPerWindow = 15;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 10;
            }));
        
        return serviceCollection;
    }
}