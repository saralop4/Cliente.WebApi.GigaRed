using Cliente.WebApi.Aplicacion.Validadores;

namespace Cliente.WebApi.Modules.Validator;


public static class ValidatorExtensions
{

    public static IServiceCollection AddValidator(this IServiceCollection services)
    {
        services.AddTransient<ClienteDtoValidador>();
        services.AddTransient<ActualizarClientePersonaDtoValidador>();


        return services;
    }
}
