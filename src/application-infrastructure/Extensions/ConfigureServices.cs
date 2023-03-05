using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using application_infrastructure.DBContexts;
using application_infrastructure.Repositories;
using application_infrastructure.Entities;
using application_infrastructure.TokenService;
using application_infrastructure.Logging;
using NLog;
using application_infrastructure.Repositories.Interfaces;
using application_infrastructure.Repositories.Implementations;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    /// <summary>
    /// It adds the Postgres DB Context, Identity Core, and the repositories to the service collection
    /// </summary>
    /// <param name="IServiceCollection">The service collection to add the services to.</param>
    /// <param name="IConfiguration">This is the configuration object that is created in the program.cs
    /// file.</param>
    /// <returns>
    /// The services collection.
    /// </returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection");
        // Add Postgres DB Context
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(conn));

        // Add Identity Core
        services
            .AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWatchLaterRepository, WatchLaterRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();

        return services;
    }

    /// <summary>
    /// It adds the JWT authentication service to the DI container
    /// </summary>
    /// <param name="IServiceCollection">This is the collection of services that will be used by the
    /// application.</param>
    /// <param name="IConfiguration">This is the configuration object that is injected into the program.cs
    /// class.</param>
    /// <returns>
    /// The services collection.
    /// </returns>
    public static IServiceCollection AddJWTService(this IServiceCollection services, IConfiguration configuration)
    {
        var validIssuer = configuration.GetSection("JWT").GetValue<string>("ValidIssuer");
        var ValidAudience = configuration.GetSection("JWT").GetValue<string>("ValidAudience");
        var Secret = configuration.GetSection("JWT").GetValue<string>("Secret");
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = validIssuer,
                    ValidAudience = ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Secret)
                    ),
                };
            });
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    /// <summary>
    /// It loads the NLog configuration file and adds the LoggerManager class to the service collection
    /// </summary>
    /// <param name="IServiceCollection">This is the interface that represents a collection of service
    /// descriptors.</param>
    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        // Setup NLog
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }
}

