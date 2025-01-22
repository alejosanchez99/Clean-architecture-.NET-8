using Restaurants.API.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Infraestructure.Extensions;
using Restaurants.Infraestructure.Seeders;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddApplication();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

WebApplication app = builder.Build();

IServiceScope scope = app.Services.CreateScope();
IRestaurantSeeder seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
