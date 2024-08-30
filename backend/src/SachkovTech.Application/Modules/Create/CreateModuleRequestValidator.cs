using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;

namespace SachkovTech.Application.Modules.Create;

public class CreateModuleRequestValidator : AbstractValidator<CreateModuleRequest>
{
    public CreateModuleRequestValidator()
    {
        RuleFor(c => c.Years).GreaterThan(0).WithError(Errors.General.ValueIsInvalid("Years"));
        RuleFor(c => c.Title).MustBeValueObject(Title.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.FullName)
            .MustBeValueObject(x => FullName.Create(x.FirstName, x.SecondName));
    }
}