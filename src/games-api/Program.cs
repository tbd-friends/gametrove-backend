using Authentication.Extensions;
using FastEndpoints;
using games_application;
using games_infrastructure_pricecharting_api;
using Microsoft.AspNetCore.Http.Timeouts;
using shared_kernel_infrastructure.Extensions;
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

builder.Services.AddFastEndpoints()
    .AddRequestTimeouts(configure =>
    {
        configure.Policies.Add("long-timeout", new RequestTimeoutPolicy { Timeout = TimeSpan.FromMinutes(2) });
    });

builder.Services.AddAuth0Authentication(builder.Configuration);

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