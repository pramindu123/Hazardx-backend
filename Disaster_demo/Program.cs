using Disaster_demo.Models;
using Disaster_demo.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // Add this for MySQL

var builder = WebApplication.CreateBuilder(args);

// ===== SERVICES CONFIGURATION =====
builder.Services.AddControllers();

// Swagger (enabled for all environments)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration - Fixed with proper MySQL setup
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DisasterDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Register all services
builder.Services.AddScoped<ISymptomsServices, SymptomsServices>();
builder.Services.AddScoped<IAidRequestServices, AidRequestServices>();
builder.Services.AddScoped<AlertServices>();
builder.Services.AddScoped<IVolunteerServices, VolunteerServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IDSOfficerServices, DSOfficerServices>();
builder.Services.AddScoped<IDMCOfficerServices, DMCOfficerServices>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IContributionService, ContributionService>();

// CORS
builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()));

// JSON Configuration
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

// Railway-specific proxy configuration
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// ===== APP BUILD =====
var app = builder.Build();

// ===== MIDDLEWARE PIPELINE =====
app.UseForwardedHeaders(); // Must be first

// Database migration - Add this to ensure database is created/migrated
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DisasterDBContext>();
    dbContext.Database.Migrate();
}

// Swagger UI in all environments
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HazardX API v1"));

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

// Minimal API endpoints
app.MapGet("/", () => Results.Redirect("/swagger")); // Redirect root to Swagger
app.MapGet("/ping", () => Results.Ok(new { Status = "OK", Time = DateTime.UtcNow }));
app.MapGet("/api/health", () => Results.Json(new { Status = "Healthy" }));

// Controllers
app.MapControllers();

// ===== START APPLICATION =====
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Logger.LogInformation($"Starting application on port {port} with connection string: {connectionString}");
app.Run($"http://0.0.0.0:{port}");