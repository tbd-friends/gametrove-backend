using Authentication.Abstractions;
using Authentication.Configuration;
using Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuth0Authentication(this IServiceCollection services, IConfiguration configuration)
    {
        var auth0Options = configuration.GetSection(Auth0Options.SectionName).Get<Auth0Options>()
                          ?? throw new InvalidOperationException($"Auth0 configuration section '{Auth0Options.SectionName}' not found");

        return services.AddAuth0Authentication(auth0Options);
    }

    public static IServiceCollection AddAuth0Authentication(this IServiceCollection services, Auth0Options options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        if (string.IsNullOrEmpty(options.Domain))
            throw new ArgumentException("Auth0 Domain is required", nameof(options));
        
        if (string.IsNullOrEmpty(options.Audience))
            throw new ArgumentException("Auth0 Audience is required", nameof(options));

        services.Configure<Auth0Options>(opt =>
        {
            opt.Domain = options.Domain;
            opt.Audience = options.Audience;
            opt.ClientId = options.ClientId;
            opt.ClientSecret = options.ClientSecret;
            opt.ManagementApiAudience = options.ManagementApiAudience;
            opt.ValidateIssuer = options.ValidateIssuer;
            opt.ValidateAudience = options.ValidateAudience;
            opt.RequireHttpsMetadata = options.RequireHttpsMetadata;
        });

        services.AddHttpClient<ITokenService, Auth0TokenService>();
        services.AddHttpClient<IAuthenticationService, Auth0AuthenticationService>();

        services.AddScoped<ITokenService, Auth0TokenService>();
        services.AddScoped<IAuthenticationService, Auth0AuthenticationService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
            {
                jwtOptions.Authority = $"https://{options.Domain}/";
                jwtOptions.Audience = options.Audience;
                jwtOptions.RequireHttpsMetadata = options.RequireHttpsMetadata;
                
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = options.ValidateIssuer,
                    ValidIssuer = $"https://{options.Domain}/",
                    ValidateAudience = options.ValidateAudience,
                    ValidAudience = options.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5),
                    NameClaimType = ClaimTypes.NameIdentifier
                };

                jwtOptions.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Auth0AuthenticationService>>();
                        
                        logger.LogDebug("Token validated for user: {UserId}", 
                            context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                        
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Auth0AuthenticationService>>();
                        
                        logger.LogWarning("Authentication failed: {Exception}", context.Exception.Message);
                        
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}