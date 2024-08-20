using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.API.Extensions;
using SachkovTech.API.Response;
using SachkovTech.Application.Modules.CreateModule;
using SachkovTech.Domain.Shared;

namespace SachkovTech.API.Controllers;

public class ModulesController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateModuleHandler handler,
        [FromServices] IValidator<CreateModuleRequest> validator,
        [FromBody] CreateModuleRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            var validationErrors = validationResult.Errors;

            var errors = from validationError in validationErrors
                let error = Error.Validation(validationError.ErrorCode, validationError.ErrorMessage)
                select new ResponseError(error.Code, error.Message, validationError.PropertyName);

            var envelope = Envelope.Error(errors);

            return BadRequest(envelope);
        }

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return CreatedAtAction("", result.Value);
    }
}