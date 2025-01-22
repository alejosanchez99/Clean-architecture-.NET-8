using Restaurants.Application.Extensions;
using Restaurants.Infraestructure.Extensions;
using Restaurants.Infraestructure.Seeders;
using Serilog;
using Serilog.Events;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning).MinimumLevel
                              .Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                              .WriteTo.File("Logs/Restaurant-API-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                              .WriteTo.Console(outputTemplate: "[{Timestamp:dd-MM HH:mm:ss} {Level:u3}] |{SourceContext}| {NewLine}{Message:lj}{NewLine}{Exception}");
});

WebApplication app = builder.Build();

IServiceScope scope = app.Services.CreateScope();
IRestaurantSeeder seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
