using FluentValidation;

namespace SachkovTech.IssuesManagement.Application.Commands.Delete;

public class DeleteModuleCommandValidator : AbstractValidator<DeleteModuleCommand>
{
    public DeleteModuleCommandValidator()
    {
        RuleFor(d => d.ModuleId).NotEmpty();
    }
}