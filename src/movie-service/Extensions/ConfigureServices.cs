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
using WatchDog;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddMoviesApiServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    public static IApplicationBuilder AddWatchDog(this IApplicationBuilder app)
    {
        app.UseWatchDogExceptionLogger();
        app.UseWatchDog(options =>
        {
            options.WatchPageUsername = "admin";
            options.WatchPagePassword = "admin";
        });

        return app;
    }
}

