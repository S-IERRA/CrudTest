using System.Text.Json.Serialization;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Test.Data.Database;
using Test.Data.Extensions;
using Test.Logic.Extensions;
using Test.Rest.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .MinimumLevel.Debug()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

builder.Services.AddMemoryCache();

builder.Services.RegisterDataServices(builder.Configuration);
builder.Services.RegisterRestServices();
builder.Services.RegisterLogicServices();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMvc();

WebApplication app = builder.Build();
app.UseCors("CORSPolicy");
app.UseRouting();
app.MapControllers();

app.UseHttpsRedirection();

app.RegisterHangfireJobs();

/*using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<EntityFrameworkContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}*/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Crud Test");
    });
}

app.Run();