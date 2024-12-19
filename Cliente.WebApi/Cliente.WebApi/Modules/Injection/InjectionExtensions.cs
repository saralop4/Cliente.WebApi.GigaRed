using Cliente.WebApi.Aplicacion.Interfaces;
using Cliente.WebApi.Aplicacion.Servicios;
using Cliente.WebApi.Dominio.Interfaces;
using Cliente.WebApi.Dominio.Persistencia;
using Cliente.WebApi.Infraestructura.Repositorios;
using Cliente.WebApi.Transversal.Interfaces;
using Cliente.WebApi.Transversal.Logging;

namespace Cliente.WebApi.Modules.Injection;

public static class InjectionExtensions
{

    public static IServiceCollection AddInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration);
        services.AddSingleton<DapperContext>();
        services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
        services.AddScoped<IClienteServicio, ClienteServicio>();
        services.AddScoped<IIpAddressService, IpAddressService>();

        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

        return services;
    }


}
