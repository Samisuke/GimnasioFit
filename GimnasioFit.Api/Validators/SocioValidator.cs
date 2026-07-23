using FluentValidation;
using GimnasioFit.Api.Dtos;

namespace GimnasioFit.Api.Validators
{
    public class SocioValidator : AbstractValidator<SocioCreateDto>
    {
        public SocioValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MinimumLength(3).WithMessage("El nombre no puede tener menos de 3 caracteres")
                .MaximumLength(15).WithMessage("El nombre no puede tener mas de 15 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email no puede estar vacio.")
                .EmailAddress().WithMessage("Introduce un correo electronico valido");

            RuleFor(x => x.Pass)
                .NotEmpty().WithMessage("La contraseña no puede estar vacio.")
                .MinimumLength(6).WithMessage("La contraseña tiene que tener minimo 6 caracteres");


        }
    }
}