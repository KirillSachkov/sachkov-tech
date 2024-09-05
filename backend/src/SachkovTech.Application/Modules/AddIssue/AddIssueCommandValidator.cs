using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects;

namespace SachkovTech.Application.Modules.AddIssue;

public class AddIssueCommandValidator : AbstractValidator<AddIssueCommand>
{
    public AddIssueCommandValidator()
    {
        RuleFor(c => c.Title).MustBeValueObject(Title.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleForEach(c => c.FileCommands).SetValidator(new CreateFileCommandValidator());
    }
}

public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
{
    public CreateFileCommandValidator()
    {
        RuleFor(c => c.FileName).MustBeValueObject(FilePath.Create);
    }
}