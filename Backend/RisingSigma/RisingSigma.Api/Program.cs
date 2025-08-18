using Microsoft.EntityFrameworkCore;
using RisingSigma.Api.Logic;
using RisingSigma.Database;

var builder = WebApplication.CreateBuilder(args);

// Add CORS service registration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddTransient<IExerciseLogic, ExerciseLogic>();
builder.Services.AddTransient<IVerificationLogic, VerificationLogic>();
builder.Services.AddSingleton<ITimeProvider, SystemTimeProvider>();
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
var environmentName = builder.Environment.EnvironmentName;

builder.Configuration
    .SetBasePath(currentDirectory)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{environmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Database"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(15),
                errorNumbersToAdd: null
            );
        }
    )
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// CORS must be the very first middleware
app.UseCors();

using (var scope = app.Services.CreateScope())
{
    try 
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
        Console.WriteLine("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database migration failed: {ex.Message}");
    }
}

// CORS must be the first middleware to handle preflight requests
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RisingSigma API V1");
    c.RoutePrefix = "";
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.Run();