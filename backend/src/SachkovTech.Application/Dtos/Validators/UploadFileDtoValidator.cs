using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Dtos.Validators;

public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
{
    public UploadFileDtoValidator()
    {
        RuleFor(u => u.FileName)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.FileName)
            .MustBeProperExtension();

        RuleFor(u => u.Content)
            .MustBeProperSize();
    }
}