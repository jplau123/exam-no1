using examNo1.domain.Configuration;
using Microsoft.OpenApi.Models;
using WebAPI.Middlewares;
using Swashbuckle.AspNetCore.Filters;

namespace exam_no1;

public static class DependencyInjection
{
    public static void AddWebApi(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<DateOptions>(config.GetSection(DateOptions.DateConfiguration));

        services.AddTransient<ErrorMiddleware>();
        services.AddTransient<AuthMiddleware>();

        services.AddSwaggerExamplesFromAssemblyOf<Program>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "ExamNo1", Version = "v1" });

            options.EnableAnnotations();
            options.ExampleFilters();

            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = "API Key header",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey",
                    },
                },
                new string[] { }
            },
            });
        });
    }
}
