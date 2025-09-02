using Authentication.Extensions;
using FastEndpoints;
using igdb_api.Infrastructure;
using igdb_application;
using igdb_infrastructure_api;
using igdb_infrastructure;
using shared_kernel_infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddApplication()
    .AddInfrastructure()
    .AddInfrastructureApi()
    .AddChannelEventBus();

builder.Services.AddFastEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AuthPolicy", policy =>
        policy.RequireAuthenticatedUser());

builder.Services.AddHostedService<DomainEventService>();

builder.Services.AddAuth0Authentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapFastEndpoints();

await app.RunAsync();