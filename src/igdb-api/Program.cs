using Authentication.Extensions;
using igdb_api.Clients;
using igdb_api.Infrastructure.Cache;
using igdb_api.Infrastructure.Cache.Fetchers;
using igdb_api.Infrastructure.Cache.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddPooledDbContextFactory<CacheDbContext>(configure =>
{
    configure.UseMongoDB(builder.Configuration.GetConnectionString("igdb-cache")!, "igdb-cache");
});

builder.Services.AddTransient<CacheDbContext>(provider =>
    provider.GetRequiredService<IDbContextFactory<CacheDbContext>>().CreateDbContext());

builder.Services.AddScoped<IIgdbCacheWrapper, CacheWrapper>();

builder.Services.AddTransient<GameFetcher>();

// Add services to the container.

builder.Services.AddControllers(options => { options.UseNamespaceRouteToken(); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddAuth0Authentication(builder.Configuration);

builder.Services.AddHttpClient<IGDBAuthClient>("igdb-auth");
builder.Services.AddHttpClient<IgdbApiClient>("igdb",
    (client) => { client.BaseAddress = new Uri(builder.Configuration["igdb:url"] ?? string.Empty); });

builder.Services.AddHostedService<CacheFetchBackgroundService>();

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

await app.RunAsync();