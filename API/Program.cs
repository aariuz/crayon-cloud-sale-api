using Microsoft.EntityFrameworkCore;
using Integrations.Crayon.Database.Models;
using API;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = null; // default; avoids $id/$ref
            options.JsonSerializerOptions.WriteIndented = true;
            //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            //options.JsonSerializerOptions.MaxDepth = 32;
        });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Crayon Cloud Sale API",
        Version = "v1",
        Description = "Crayon exercise API",
    });

    // Ensure that the OpenAPI version is 3.x
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});

var mdfFolderPath = Path.Combine(AppContext.BaseDirectory, "Crayon", "Database");
AppDomain.CurrentDomain.SetData("DataDirectory", mdfFolderPath);

// Register DbContext
builder.Services.AddDbContext<CrayonDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration["DbConnectionString"]));

// Register AutoMapper and scan for profiles
builder.Services.AddAutoMapper(typeof(Program));

TypeRegistrations.RegisterTypes(builder.Services);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:APIKey"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
