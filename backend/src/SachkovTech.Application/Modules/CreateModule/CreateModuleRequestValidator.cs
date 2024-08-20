using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Modules;

namespace SachkovTech.Application.Modules.CreateModule;

public class CreateModuleRequestValidator : AbstractValidator<CreateModuleRequest>
{
    public CreateModuleRequestValidator()
    {
        RuleFor(c => c.Title).MustBeValueObject(Title.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
    }
}