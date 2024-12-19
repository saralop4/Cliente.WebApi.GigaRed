using Cliente.WebApi.Dominio.DTOs;
using FluentValidation;

namespace Cliente.WebApi.Aplicacion.Validadores;

public class ClienteDtoValidador : AbstractValidator<ClienteDto>
{
    public ClienteDtoValidador()
    {

        RuleFor(u => u.IdIndicativo)
             .NotEmpty().WithMessage("Debe seleccionar el indicativo.")
             .NotNull().WithMessage("El indicativo no puede ser nulo.");

        RuleFor(u => u.IdCiudad)
            .NotEmpty().WithMessage("Debe seleccionar la ciudad.")
            .NotNull().WithMessage("El indicativo no puede ser nulo.");

        RuleFor(u => u.PrimerNombre)
            .NotEmpty().WithMessage("El primer nombre es obligatorio.")
            .NotNull().WithMessage("El primer nombre no puede ser nulo.")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El primer nombre solo puede contener letras.");

        RuleFor(u => u.SegundoNombre)
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El segundo nombre solo puede contener letras.");

        RuleFor(u => u.PrimerApellido)
            .NotEmpty().WithMessage("El primer apellido es obligatorio.")
            .NotNull().WithMessage("El primer apellido no puede ser nulo.")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El primer apellido solo puede contener letras.");

        RuleFor(u => u.SegundoApellido)
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El segundo apellido solo puede contener letras.");

        RuleFor(u => u.Telefono)
            .NotEmpty().WithMessage("El telefono es obligatorio.")
            .NotNull().WithMessage("El telefono no puede ser nulo.")
            .Must(SoloNumeros).WithMessage("El telefono solo puede contener números.");

        RuleFor(u => u.UsuarioQueRegistra)
            .NotEmpty().WithMessage("El usuario que registra  es obligatorio.")
            .NotNull().WithMessage("El usuario que registra  no puede ser nulo.")
            .Length(10, 80).WithMessage("El correo debe ser entre 10 y  80 caracteres.")
            .EmailAddress().WithMessage("El usuario que registra debe tener un formato válido. (ejemplo@dominio.com)");
        
        RuleFor(u => u.IpDeRegistro)
                .NotEmpty().WithMessage("La IP de registro es obligatoria.")
                .NotNull().WithMessage("La IP de registro no puede ser nula.");

    }
    private bool SoloNumeros(string? telefono) // Acepta nulos
    {
        // Si telefono es nulo, retorna false
        if (telefono == null) return false;

        return telefono.All(char.IsDigit);
    }

}
