using Authentication.Extensions;
using FastEndpoints;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TbdDevelop.GameTrove.GameApi.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFastEndpoints();

builder.Services.AddMemoryCache();

builder.Services.AddPooledDbContextFactory<GameTrackingContext>((provider, configure) =>
{
    var cache = provider.GetRequiredService<IMemoryCache>();

    configure.UseSqlServer(builder.Configuration.GetConnectionString("gametracking-work"))
        .UseMemoryCache(cache)
        .LogTo(Console.WriteLine);
});

builder.Services.AddAuth0Authentication(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthPolicy", policy =>
        policy.RequireAuthenticatedUser());
});

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

await app.RunAsync();