using FluentValidation;

namespace SachkovTech.Issues.Application.IssueManagement.Commands.Delete;

public class DeleteModuleCommandValidator : AbstractValidator<DeleteModuleCommand>
{
    public DeleteModuleCommandValidator()
    {
        RuleFor(d => d.ModuleId).NotEmpty();
    }
}