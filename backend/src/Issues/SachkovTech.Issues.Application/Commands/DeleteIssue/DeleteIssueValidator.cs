using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Commands.DeleteIssue;

public class DeleteIssueValidator : AbstractValidator<DeleteIssueCommand>
{
    public DeleteIssueValidator()
    {
        RuleFor(u => u.ModuleId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.IssueId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}