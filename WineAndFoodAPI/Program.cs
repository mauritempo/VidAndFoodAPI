using Application.Interfaces;
using Application.Options;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure;
using Infrastructure.Options;
using Infrastructure.Repository;
using Infrastructure.Security;
using Infrastructure.Services;
using Infrastructure.Services.Resilience;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var fromApp = builder.Configuration["Gemini:ApiKey"];

string connectionString = builder.Configuration["ConnectionStrings:WineAndFoodDBConnectionString"]!;
var connection = new SqliteConnection(connectionString);
connection.Open();

builder.Services.Configure<GeminiOptions>(o =>
{
    // Prefer� env var si existe
    var fromEnv = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
    if (!string.IsNullOrWhiteSpace(fromEnv))
        o.ApiKey = fromEnv;

    // appsettings fallback
    var fromApp = builder.Configuration["Gemini:ApiKey"];
    if (!string.IsNullOrWhiteSpace(fromApp) && string.IsNullOrWhiteSpace(o.ApiKey))
        o.ApiKey = fromApp;

    // Modelo (opcional)
    o.Model = builder.Configuration["Gemini:Model"] ?? "gemini-1.5-flash";

    if (string.IsNullOrWhiteSpace(o.ApiKey))
        throw new InvalidOperationException("Google Gemini API key is not set. Configure 'GOOGLE_API_KEY' env var or 'Gemini:ApiKey' in appsettings.");
});

builder.Services.AddHttpClient("Gemini", client =>
{
    // Si tu SDK maneja las URLs, no sete�s BaseAddress.
    // Si NO, configur� ac� el endpoint base del servicio que use el SDK.
    client.Timeout = TimeSpan.FromSeconds(20);
})
.AddPolicyHandler(PollyResiliencePolicies.GetCircuitBreakerPolicy())
.AddPolicyHandler(PollyResiliencePolicies.GetRetryPolicy());

builder.Services.AddCors();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("VidAndFoodAPIBearerAuth", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Ac� pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "VidAndFoodAPIBearerAuth" 
                } //Tiene que coincidir con el id seteado arriba en la definici�n
                }, new List<string>() }
    });
});
builder.Configuration.AddEnvironmentVariables();


builder.Services.AddAuthorization();



#region Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWineRepository, WineRepository>();
builder.Services.AddScoped<IGrapeRepository, GrapeRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IWineUserRepository, WineUserRepository>();
builder.Services.AddScoped<IWineFavouriteRepository, WineFavouriteRepository>();
builder.Services.AddScoped<ICellarItemRepository, CellarItemRepository>();
builder.Services.AddScoped<ICellarRepository, CellarRepository>();

#endregion

#region Services
// Registra el servicio y pasa la API key y el cliente configurado
builder.Services.AddSingleton<IGeminiApiService, GeminiAPIService>();
#endregion

;

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowOrigin",
        policyBuilder =>
        {
            policyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173"));
app.UseAuthorization();

app.MapControllers();

app.Run();
