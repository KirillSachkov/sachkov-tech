// using FluentValidation;
// using SachkovTech.Core;
// using SachkovTech.Core.Validation;
// using SachkovTech.Core.Validators;
// using SachkovTech.SharedKernel;
//
// namespace SachkovTech.IssuesManagement.Application.Commands.UploadFilesToIssue;
//
// public class UploadFilesToIssueCommandValidator : AbstractValidator<UploadFilesToIssueCommand>
// {
//     public UploadFilesToIssueCommandValidator()
//     {
//         RuleFor(u => u.ModuleId).NotEmpty().WithError(Errors.General.ValueIsRequired());
//         RuleFor(u => u.IssueId).NotEmpty().WithError(Errors.General.ValueIsRequired());
//         RuleForEach(u => u.Files).SetValidator(new UploadFileDtoValidator());
//     }
// }