using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.IssuesReviews.Contracts;
using SachkovTech.IssuesReviews.Contracts.Requests;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueSolving.Application.Commands.SendOnReview;

public class SendOnReviewHandler : ICommandHandler<SendOnReviewCommand>
{
    private readonly IUserIssueRepository _repository;
    private readonly ILogger<SendOnReviewHandler> _logger;
    private readonly IIssuesReviewsContract _contract;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SendOnReviewCommand> _validator;

    public SendOnReviewHandler(IUserIssueRepository repository,
        ILogger<SendOnReviewHandler> logger,
        IIssuesReviewsContract contract,
        [FromKeyedServices(Modules.IssueSolving)] IUnitOfWork unitOfWork,
        IValidator<SendOnReviewCommand> validator)
    {
        _repository = repository;
        _logger = logger;
        _contract = contract;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(SendOnReviewCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }
        
        var userIssue = await _repository
            .GetUserIssueById(UserIssueId.Create(command.UserIssueId), cancellationToken);

        if (userIssue.IsFailure)
        {
            _logger.LogError("UserIssue with {Id} not found", command.UserIssueId);
            return userIssue.Error.ToErrorList();
        }

        var pullRequestUrl = PullRequestUrl.Create(command.PullRequestUrl).Value;

        var sendOnReviewRes = userIssue.Value.SendOnReview(pullRequestUrl);

        if (sendOnReviewRes.IsFailure)
        {
            return sendOnReviewRes.Error.ToErrorList();
        }

        var createIssueReviewRes = await _contract.CreateIssueReview(command.UserIssueId, command.UserId,
            new CreateIssueReviewRequest(command.PullRequestUrl), cancellationToken);

        if (createIssueReviewRes.IsFailure)
        {
            return createIssueReviewRes.Error;
        }

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Issue with UserIssueId {UserIssueId} was created", command.UserIssueId);

        return UnitResult.Success<ErrorList>();
    }
}