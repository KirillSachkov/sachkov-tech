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
            .WithError(Errors.Files.InvalidExtension());
        
        RuleFor(u => u.FileName)
            .Must(fn =>
                Constants.Files.ALLOWED_TEXT_EXTENSIONS
                    .FirstOrDefault(ext => ext == Path.GetExtension(fn)) is not null ||
                Constants.Files.ALLOWED_PHOTO_EXTENSIONS
                    .FirstOrDefault(ext => ext == Path.GetExtension(fn)) is not null
            )
            .WithError(Errors.Files.InvalidExtension());

        RuleFor(u => u.Content)
            .Must(c => c.Length < Constants.Files.MAX_FILE_SIZE)
            .WithError(Errors.Files.InvalidSize());
    }
}