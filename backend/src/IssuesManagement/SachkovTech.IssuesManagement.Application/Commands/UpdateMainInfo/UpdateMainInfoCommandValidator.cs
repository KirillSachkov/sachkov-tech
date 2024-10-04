using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.IssuesManagement.Application.Commands.UpdateMainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(r => r.ModuleId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Title).MustBeValueObject(Title.Create);
        RuleFor(r => r.Description).MustBeValueObject(Description.Create);
    }
}