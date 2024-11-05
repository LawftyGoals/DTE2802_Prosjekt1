using CMSAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure JSON serialization to handle reference loops
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddDbContext<CMSAPIDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // Register the DbContext with SQLite as the database

builder.Services.AddScoped<CMSAPIDbContext>();

builder.Services.AddEndpointsApiExplorer(); // Enable endpoint exploration for minimal APIs
builder.Services.AddSwaggerGen(); // Add Swagger for API documentation

// Add CORS policy to allow cross-origin requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Allow requests from any origin
            .AllowAnyMethod() // Allow any HTTP method
            .AllowAnyHeader(); // Allow any HTTP header
    });
});

var app = builder.Build();

// Initialize SQLite PCL for SQLite provider setup
Batteries.Init(); // Ensures proper SQLite initialization

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger in development environment
    app.UseSwaggerUI(); // Enable Swagger UI
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseStaticFiles(); // Serve static files from wwwroot

app.UseCors("AllowAll"); // Enable the CORS policy defined earlier

app.UseAuthentication(); // Use authentication middleware
app.UseAuthorization(); // Use authorization middleware

app.MapControllers(); // Map controller routes to handle requests

// Apply migrations and seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CMSAPIDbContext>();
    context.Database.Migrate(); // Apply any pending migrations
    SeedData.Initialize(services); // Seed the database with initial data
}

app.Run(); // Run the application
