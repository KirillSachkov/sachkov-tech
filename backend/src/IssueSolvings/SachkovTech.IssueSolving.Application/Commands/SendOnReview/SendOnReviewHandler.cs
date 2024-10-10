using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.IssueSolving.Domain.ValueObjects;
using SachkovTech.IssuesReviews.Contracts;
using SachkovTech.IssuesReviews.Contracts.Requests;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueSolving.Application.Commands.SendOnReview;

public class SendOnReviewHandler : ICommandHandler<SendOnReviewCommand>
{
    private readonly IUserIssueRepository _repository;
    private readonly ILogger<SendOnReviewHandler> _logger;
    private readonly IIssuesReviewsContract _contract;
    private readonly IUnitOfWork _unitOfWork;

    public SendOnReviewHandler(IUserIssueRepository repository,
        ILogger<SendOnReviewHandler> logger,
        IIssuesReviewsContract contract,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _contract = contract;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Handle(SendOnReviewCommand command,
        CancellationToken cancellationToken = default)
    {
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

        if (createIssueReviewRes.Error.Any())
        {
            return createIssueReviewRes.Error;
        }

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Issue with UserIssueId {UserIssueId} was created", command.UserIssueId);

        return UnitResult.Success<ErrorList>();
    }
}