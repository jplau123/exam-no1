using examNo1.application.Interfaces;
using examNo1.application.Mappers;
using examNo1.application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace examNo1.application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IExchangeRateService, ExchangeRateService>();

        // Automapper
        services.AddAutoMapper(typeof(AutoMapperProfile));
    }
}
