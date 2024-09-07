using FluentValidation;
using SachkovTech.Application.Dtos.Validators;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Modules.UploadFilesToIssue;

public class UploadFilesToIssueCommandValidator : AbstractValidator<UploadFilesToIssueCommand>
{
    public UploadFilesToIssueCommandValidator()
    {
        RuleFor(u => u.ModuleId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.IssueId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(u => u.Files).SetValidator(new UploadFileDtoValidator());
    }
}