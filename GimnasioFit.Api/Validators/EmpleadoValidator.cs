using FluentValidation;
using GimnasioFit.Api.Dtos;

namespace GimnasioFit.Api.Validators
{
    public class EmpleadoValidator : AbstractValidator<EmpleadoCreateDto>
    {
        public EmpleadoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre no puede estar vacio.")
                .MinimumLength(3).WithMessage("El nombre no puede tener menos de tres letras")
                .MaximumLength(15).WithMessage("El nombre no puede tener mas de 15 letras.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email no puede estar vacio.")
                .EmailAddress().WithMessage("Introduce un correo electronico valido");

            RuleFor(x => x.Pass)
                .NotEmpty().WithMessage("La contraseña no puede estar vacio.")
                .MinimumLength(6).WithMessage("La contraseña tiene que tener minimo 6 caracteres");
            
            RuleFor(x => x.Puesto)
                .NotEmpty().WithMessage("El puesto de trabajo es obligatorio.");
           
            RuleFor(x => x.NivelAcceso)
                .InclusiveBetween(1, 3).WithMessage("El nivel de acceso debe ser 1 (Profesor), 2 (Gestor) o 3 (Admin).");
        }
    }
}