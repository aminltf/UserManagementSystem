using Microsoft.AspNetCore.Mvc;

namespace UserManagement.WebAPI.Extensions;

public static class ApiVersioningExtension
{
    public static IServiceCollection RegisterApiVersioningExtension(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.UrlSegmentApiVersionReader();
        });

        return services;
    }
}
