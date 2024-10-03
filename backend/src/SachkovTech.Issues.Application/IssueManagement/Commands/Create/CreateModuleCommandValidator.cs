using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.Core.ValueObjects;

namespace SachkovTech.Issues.Application.IssueManagement.Commands.Create;

public class CreateModuleCommandValidator : AbstractValidator<CreateModuleCommand>
{
    public CreateModuleCommandValidator()
    {
        RuleFor(c => c.Title).MustBeValueObject(Title.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
    }
}