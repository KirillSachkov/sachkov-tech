using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared.ValueObjects;

namespace SachkovTech.Application.IssueManagement.Commands.Create;

public class CreateModuleCommandValidator : AbstractValidator<CreateModuleCommand>
{
    public CreateModuleCommandValidator()
    {
        RuleFor(c => c.Title).MustBeValueObject(Title.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
    }
}