using Cliente.WebApi.Dominio.DTOs;
using FluentValidation;

namespace Cliente.WebApi.Aplicacion.Validadores;

public class ActualizarClientePersonaDtoValidador : AbstractValidator<ClienteDto>
{
    public ActualizarClientePersonaDtoValidador()
    {
        RuleFor(u => u.IdCliente)
            .NotEmpty().WithMessage("El idCliente no puede estar vacio.")
            .NotNull().WithMessage("El idCliente no puede ser nulo.")
            .Must(SoloNumerosEnteros).WithMessage("El idCliente solo puede contener números.")
            .GreaterThan(0).WithMessage("Debe proporcionar un idCliente válido y mayor que 0."); ;

        RuleFor(u => u.IdPersona)
            .NotEmpty().WithMessage("El idPersona no puede estar vacio.")
            .NotNull().WithMessage("El idPersona no puede ser nulo.")
            .Must(SoloNumerosEnteros).WithMessage("El idPersona solo puede contener números.")
            .GreaterThan(0).WithMessage("Debe proporcionar un idPersona válido y mayor que 0.");


        RuleFor(u => u.PrimerNombre)
           .NotEmpty().WithMessage("El primer nombre no puede estar vacio.")
           .NotNull().WithMessage("El primer nombre no puede ser nulo.")
           .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El primer nombre solo puede contener letras.");

        RuleFor(u => u.SegundoNombre)
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El segundo nombre solo puede contener letras.");

        RuleFor(u => u.PrimerApellido)
            .NotEmpty().WithMessage("El primer apellido no puede estar vacio.")
            .NotNull().WithMessage("El primer apellido no puede ser nulo.")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El primer apellido solo puede contener letras.");

        RuleFor(u => u.SegundoApellido)
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ]+$").WithMessage("El segundo apellido solo puede contener letras.");

        RuleFor(u => u.Telefono)
            .NotEmpty().WithMessage("El telefono no puede estar vacio.")
            .NotNull().WithMessage("El telefono no puede ser nulo.")
            .Matches(@"^\d+$").WithMessage("El telefono solo puede contener números.")
            .Length(7, 15).WithMessage("El telefono debe tener entre 7 y 15 dígitos.");

        RuleFor(u => u.UsuarioQueActualiza)
            .NotEmpty().WithMessage("El usuario que actualiza  es obligatorio.")
            .NotNull().WithMessage("El usuario que actualiza  no puede ser nulo.")
            .MaximumLength(80).WithMessage("El usuario que actualiza  no puede tener más de 80 caracteres.")
            .EmailAddress().WithMessage("El usuario que actualiza  debe tener un formato válido. (ejemplo@dominio.com)");

        RuleFor(u => u.IpDeActualizado)
            .NotEmpty().WithMessage("La ip de actualizado  es obligatorio.")
            .NotNull().WithMessage("La ip de actualizado  no puede ser nulo.");


    }


    private bool SoloNumerosEnteros(long telefono)
    {
        return telefono.ToString().All(char.IsDigit);
    }




}
