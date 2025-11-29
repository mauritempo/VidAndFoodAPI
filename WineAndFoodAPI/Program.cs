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
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Database
builder.Services.AddDbContext<WineDBContext>(dbContextOptions =>
    dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:WineAndFoodDBConnectionString"]));

#endregion



#region Options (bind seguro)
var geminiSection = builder.Configuration.GetSection(GeminiOptions.SectionName);

builder.Services
    .AddOptions<GeminiOptions>()
    .Bind(geminiSection)
    .PostConfigure(o =>
    {
        
        var fromEnv = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
        if (!string.IsNullOrWhiteSpace(fromEnv))
            o.ApiKey = fromEnv;

        
        o.Model ??= "gemini-2.5-flash";
        o.BaseUrl ??= "https://generativelanguage.googleapis.com/v1beta/";
    })
    .ValidateDataAnnotations()
    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "Gemini ApiKey is required.")
    .Validate(o => Uri.TryCreate(o.BaseUrl, UriKind.Absolute, out _), "Gemini BaseUrl must be absolute.")
    .ValidateOnStart();
#endregion



#region Services external APIs (Gemini)
ApiClientConfiguration geminiApiRelisienceConfig = new()
{
    RetryCount = 2,
    RetryAttemptInSeconds = 30,
    DurationOfBreakInSeconds = 50,
    HandleEventsAllowedBeforeBreaking = 10
};

//generar un objt
builder.Services.AddHttpClient("geminiHttpClient")
    .ConfigureHttpClient((sp, client) =>
    {
        var opts = sp.GetRequiredService<IOptions<GeminiOptions>>().Value;
        client.BaseAddress = new Uri(opts.BaseUrl);
        
    })
    .AddPolicyHandler(PollyResiliencePolicies.GetRetryPolicy(geminiApiRelisienceConfig))
    .AddPolicyHandler(PollyResiliencePolicies.GetCircuitBreakerPolicy(geminiApiRelisienceConfig));
#endregion

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

var secret = builder.Configuration["Jwt:Secret"]; // Jwt__Secret
if (string.IsNullOrWhiteSpace(secret))
    throw new InvalidOperationException("Configura la variable de entorno Jwt__Secret.");

// DI security
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
// Password hasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
// JWT Bearer
var issuer = builder.Configuration["Jwt:Issuer"] ?? "VidAndFood.Api";
var audience = builder.Configuration["Jwt:Audience"] ?? "VidAndFood.Client";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(o =>
  {
      o.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = key,
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidIssuer = issuer,
          ValidAudience = audience,
      };
  });


#region Services Application
builder.Services.AddSingleton<IGeminiClient,GeminiApiService>();
builder.Services.AddScoped<IUserService,UserServices>();
builder.Services.AddScoped<IWineService,WineService>();
builder.Services.AddScoped<ICellarPhysicsService, CellarPhysicsService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IWineUserService, WineUserService>();

#endregion


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
