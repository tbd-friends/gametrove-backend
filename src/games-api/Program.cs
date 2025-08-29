using Authentication.Extensions;
using FastEndpoints;
using games_application;
using shared_kernel_infrastructure.Extensions;
using TbdDevelop.GameTrove.Games.Infrastructure;

var builder = WebApplication
    .CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddInfrastructure();
builder.AddApplication();
builder.AddSecretStore();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFastEndpoints();

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