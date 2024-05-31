using API.Configuration;
using API.Middleware;
using Application;
using Infrastructure;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfig();

builder.Services.AddApplicationDb(builder.Configuration);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetry(
                retryCount: 10, 
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Attempt {retryCount} failed: {exception.Message}. Retrying in {timeSpan}...");
                });

        retryPolicy.Execute(() =>
        {
            context.Database.Migrate();
            SeedDatabaseAsync(app).Wait();
        });
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying database migrations");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Globe API");
        c.DefaultModelsExpandDepth(-1);  
    });
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("http://localhost:3000", "https://localhost:3000")); 
}
else
{
    var corsOrigins = new List<string>();
    if (Environment.GetEnvironmentVariable("ALLOW_DEV_ORIGIN") == "true")
    {
        corsOrigins.Add("http://localhost:3000");
    }

    app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(corsOrigins.ToArray()));
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

async Task SeedDatabaseAsync(IHost app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        await SeedData.SeedDataAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}

public partial class Program() { }