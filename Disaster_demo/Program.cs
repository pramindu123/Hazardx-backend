using Disaster_demo.Models;
using Disaster_demo.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var MyAllowSpecificOrigins = "*";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration
builder.Services.AddDbContext<DisasterDBContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("defaultDB"));
});

// Register all your services
builder.Services.AddScoped<ISymptomsServices, SymptomsServices>();
builder.Services.AddScoped<IAidRequestServices, AidRequestServices>();
builder.Services.AddScoped<AlertServices>();
builder.Services.AddScoped<IVolunteerServices, VolunteerServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IDSOfficerServices, DSOfficerServices>();
builder.Services.AddScoped<IDMCOfficerServices, DMCOfficerServices>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IContributionService, ContributionService>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// JSON Configuration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) // Show Swagger in both environments
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Railway-specific middleware configuration
app.UseForwardedHeaders(); // Important for Railway's reverse proxy

app.UseCors(MyAllowSpecificOrigins);

// Only use HTTPS redirection if not in development
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

// Get PORT from environment variable or use default
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");