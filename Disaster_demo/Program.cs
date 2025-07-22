using Disaster_demo.Models;
using Disaster_demo.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Constants
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container
builder.Services.AddControllers();

// Swagger configuration (enabled for both development and production)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration
builder.Services.AddDbContext<DisasterDBContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("defaultDB"));
});

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

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// JSON Configuration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

// Forwarded Headers (Critical for Railway)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Middleware Pipeline
app.UseForwardedHeaders(); // Must be first

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();

// Get PORT from Railway environment or use default
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");