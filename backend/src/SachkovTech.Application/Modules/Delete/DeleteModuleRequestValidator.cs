using FluentValidation;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace SachkovTech.Application.Modules.Delete;

public class DeleteModuleRequestValidator : AbstractValidator<DeleteModuleRequest>
{
    public DeleteModuleRequestValidator()
    {
        RuleFor(d => d.ModuleId).NotEmpty();
    }
}