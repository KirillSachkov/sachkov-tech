using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Commands.UpdateIssuePosition;

public class UpdateIssuePositionValidator : AbstractValidator<UpdateIssuePositionCommand>
{
    public UpdateIssuePositionValidator()
    {
        RuleFor(u => u.ModuleId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.IssueId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.NewPosition).GreaterThanOrEqualTo(1).LessThanOrEqualTo(1000);
    }
}