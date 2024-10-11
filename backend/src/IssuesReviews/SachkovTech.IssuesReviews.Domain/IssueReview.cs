using CSharpFunctionalExtensions;
using SachkovTech.IssuesReviews.Domain.Entities;
using SachkovTech.IssuesReviews.Domain.Enums;
using SachkovTech.IssuesReviews.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Domain;

public sealed class IssueReview : Entity<IssueReviewId>
{
    // ef core
    private IssueReview(IssueReviewId id) : base(id)
    {
    }

    public IssueReview(
        IssueReviewId issueReviewId,
        UserIssueId userIssueId,
        UserId userId,
        IssueReviewStatus issueReviewStatus,
        DateTime reviewStartedTime,
        DateTime? issueApprovedTime,
        PullRequestLink pullRequestLink)
        : base(issueReviewId)
    {
        UserIssueId = userIssueId;
        UserId = userId;
        IssueReviewStatus = issueReviewStatus;
        ReviewStartedTime = reviewStartedTime;
        IssueApprovedTime = issueApprovedTime;
        PullRequestLink = pullRequestLink;
    }

    public UserIssueId UserIssueId { get; private set; }
    public UserId UserId { get; private set; }

    public UserId? ReviewerId { get; private set; } = null;

    public IssueReviewStatus IssueReviewStatus { get; private set; }

    private List<Comment> _comments = [];
    public IReadOnlyList<Comment> Comments => _comments;

    public DateTime ReviewStartedTime { get; private set; }
    public DateTime? IssueTakenTime { get; private set; }

    public DateTime? IssueApprovedTime { get; private set; }

    public PullRequestLink PullRequestLink { get; private set; }

    public void StartReview(UserId reviewerId)
    {
        ReviewerId = reviewerId;
        IssueReviewStatus = IssueReviewStatus.OnReview;

        if (IssueTakenTime == null)
        {
            IssueTakenTime = DateTime.UtcNow;
        }
    }

    public UnitResult<Error> SendIssueForRevision()
    {
        if (IssueReviewStatus != IssueReviewStatus.OnReview)
        {
            return Errors.General.ValueIsInvalid("issue-review-status");
        }

        IssueReviewStatus = IssueReviewStatus.AskedForRevision;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Approve()
    {
        if (IssueReviewStatus != IssueReviewStatus.OnReview)
        {
            return Errors.General.ValueIsInvalid("issue-review-status");
        }

        IssueReviewStatus = IssueReviewStatus.Accepted;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddComment(Comment comment)
    {
        if (comment.UserId != UserId || (ReviewerId != null && ReviewerId != comment.UserId))
        {
            return Errors.General.ValueIsInvalid("userId");
        }

        _comments.Add(comment);

        return UnitResult.Success<Error>();
    }
}