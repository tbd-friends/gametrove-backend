using Authentication.Extensions;
using FastEndpoints;
using games_application;
using games_infrastructure_pricecharting_api;
using Microsoft.Extensions.Http.Resilience;
using shared_kernel_infrastructure.Extensions;
using TbdDevelop.GameTrove.GameApi.Infrastructure;
using TbdDevelop.GameTrove.Games.Infrastructure;

var builder = WebApplication
    .CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddInfrastructure();
builder.AddApplication();
builder.AddSecretStore();
builder.AddPriceChartingInfrastructure();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFastEndpoints();

builder.Services.ConfigureHttpClientDefaults(http =>
{
    http.AddStandardResilienceHandler(options =>
    {
        var timeoutStrategy = new HttpTimeoutStrategyOptions
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        options.AttemptTimeout = timeoutStrategy;
        options.TotalRequestTimeout = timeoutStrategy;
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromMinutes(10);

        options.Retry.MaxRetryAttempts = 2;
    });
});

builder.Services.AddAuth0Authentication(builder.Configuration);

builder.Services.AddHostedService<DomainEventService>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AuthPolicy", policy =>
        policy.RequireAuthenticatedUser());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

await app.RunAsync();