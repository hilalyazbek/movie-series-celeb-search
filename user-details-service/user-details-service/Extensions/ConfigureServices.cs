using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using user_details_service.Infrastructure.DBContexts;
using user_details_service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection");
        // Add Postgres DB Context
        services.AddDbContext<UsersContext>(options =>
            options.UseNpgsql(conn));

        // Add Identity Core
        services
            .AddIdentityCore<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<UsersContext>();

        return services;
    }

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
        services.AddScoped<TokenService, TokenService>();

        return services;

    }
}

