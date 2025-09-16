using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Options;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;

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



#region Services Application
builder.Services.AddSingleton<IGeminiClient,GeminiApiService>();
#endregion


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



#region Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion

;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
