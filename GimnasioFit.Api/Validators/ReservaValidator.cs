using FluentValidation;
using GimnasioFit.Api.Dtos;

namespace GimnasioFit.Api.Validators
{
    public class ReservaValidator : AbstractValidator<ReservaCreateDto>
    {
        public ReservaValidator()
        {
            RuleFor (x => x.ClaseId)
                .NotEmpty().WithMessage("Este campo no puede estar vacio.");
        }
    }
}