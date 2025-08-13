using Authentication.Extensions;
using igdb_api.Clients;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers(options => { options.UseNamespaceRouteToken(); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddAuth0Authentication(builder.Configuration);

builder.Services.AddHttpClient<IGDBAuthClient>("igdb-auth");
builder.Services.AddHttpClient<IGDBApiClient>("igdb",
    (client) => { client.BaseAddress = new Uri(builder.Configuration["igdb:url"] ?? string.Empty); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();