using FluentValidation;
using SachkovTech.Application.Dtos.Validators;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.IssueManagement.Commands.UpdatePosition;

public class UpdateIssuePositionValidator : AbstractValidator<UpdateIssuePositionCommand>
{
    public UpdateIssuePositionValidator()
    {
        RuleFor(u => u.ModuleId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.IssueId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.NewPosition).GreaterThanOrEqualTo(1).LessThanOrEqualTo(1000);
    }
}