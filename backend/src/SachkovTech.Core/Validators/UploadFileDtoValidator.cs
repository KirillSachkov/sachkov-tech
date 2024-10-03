using FluentValidation;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Validation;

namespace SachkovTech.Core.Validators;

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