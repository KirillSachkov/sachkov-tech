using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Issues.Application.Commands.AddIssue;

public class AddIssueCommandValidator : AbstractValidator<AddIssueCommand>
{
    public AddIssueCommandValidator()
    {
        RuleFor(c => c.Title).MustBeValueObject(Title.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
    }
}