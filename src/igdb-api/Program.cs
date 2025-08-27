using Authentication.Extensions;
using FastEndpoints;
using igdb_application;
using igdb_infrastructure_api;
using igdb_infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddApplication()
    .AddInfrastructure()
    .AddInfrastructureApi();

builder.Services.AddFastEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AuthPolicy", policy =>
        policy.RequireAuthenticatedUser());

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