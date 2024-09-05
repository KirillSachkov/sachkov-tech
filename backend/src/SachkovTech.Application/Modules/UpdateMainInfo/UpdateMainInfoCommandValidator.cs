using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;

namespace SachkovTech.Application.Modules.UpdateMainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(r => r.ModuleId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Title).MustBeValueObject(Title.Create);
        RuleFor(r => r.Description).MustBeValueObject(Description.Create);
    }
}