using Audit.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace user_details_service.Helpers;

public static class AuditConfiguration
{
    // Enables audit log with a global Action Filter
    public static void AddAudit(MvcOptions mvcOptions)
    {
        mvcOptions.AddAuditFilter(config => config
            .LogAllActions()
            .WithEventType("{verb} {controller}.{action}")
            .IncludeHeaders()
            .IncludeRequestBody()
            .IncludeResponseHeaders()
        );
    }

    // Configures what and how is logged or is not logged
    public static void ConfigureAudit(IServiceCollection serviceCollection)
    {
        //Audit.Core.Configuration.Setup()
        //    .UseFileLogProvider(config => config
        //       .( ev => Console.WriteLine(ev.ToJson())));
    }
}
