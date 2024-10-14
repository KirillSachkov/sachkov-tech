using System;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Application.Commands.StartReview;

public class StartReviewHandler : ICommandHandler<Guid, StartReviewCommand>
{
    private readonly IIssueReviewRepository _issueReviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<StartReviewCommand> _validator;
    private readonly ILogger<StartReviewHandler> _logger;

    public StartReviewHandler(
        IIssueReviewRepository issueReviewRepository,
        [FromKeyedServices(Modules.IssuesReviews)] IUnitOfWork unitOfWork,
        IValidator<StartReviewCommand> validator,
        ILogger<StartReviewHandler> logger)
    {
        _issueReviewRepository = issueReviewRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        StartReviewCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var issueReviewResult = await _issueReviewRepository
            .GetById(IssueReviewId.Create(command.IssueReviewId), cancellationToken);

        if (issueReviewResult.IsFailure)
            return issueReviewResult.Error.ToErrorList();

        issueReviewResult.Value.StartReview(UserId.Create(command.ReviewerId));
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "IssueReview {issueReviewId} started by user {userId}",
            issueReviewResult.Value.Id.Value,
            issueReviewResult.Value.UserId.Value);

        return issueReviewResult.Value.Id.Value;
    }
}
