using System.Text;
using InventoryManagement.API.Middleware;
using InventoryManagement.Application;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Infrastructure;
using InventoryManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ===== Service Registration =====

// Add Controllers
builder.Services.AddControllers();

// Add Application Layer Services (MediatR, FluentValidation, etc.)
builder.Services.AddApplication();

// Add Infrastructure Layer Services (DbContext, Repositories, External Services)
builder.Services.AddInfrastructure(builder.Configuration);

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Configure Swagger/OpenAPI with JWT Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Inventory Management API",
        Version = "v1",
        Description = "A Clean Architecture API for Inventory Management with JWT Authentication"
    });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure CORS Policy for React/Frontend Applications
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000",      // CRA default
                    "http://localhost:5173",      // Vite default
                    "http://localhost:7174",      // Your actual port ✅
                    "https://localhost:7174")     // HTTPS version ✅
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// ===== Build Application =====
var app = builder.Build();

// ===== Database Seeding =====
// Seed default admin user when application starts
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = services.GetRequiredService<IPasswordHasher>();
        
        await ApplicationDbContextSeed.SeedDefaultUserAsync(context, passwordHasher);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// ===== Middleware Pipeline =====

// Enable Swagger in Development Environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS Redirection
app.UseHttpsRedirection();

// CORS - Must be before Authentication and Authorization
app.UseCors("AllowReactApp");

// Global Exception Handling Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Authentication - Validates JWT tokens
app.UseAuthentication();

// Authorization - Checks user permissions
app.UseAuthorization();

// Map Controller Endpoints
app.MapControllers();

// ===== Run Application =====
app.Run();

// Make the implicit Program class public for integration tests
public partial class Program { }
