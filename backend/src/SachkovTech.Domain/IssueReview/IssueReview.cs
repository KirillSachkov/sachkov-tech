using CSharpFunctionalExtensions;
using SachkovTech.Core;
using SachkovTech.Core.ValueObjects.Ids;
using SachkovTech.Domain.IssueReview.Entities;
using SachkovTech.Domain.IssueReview.Other;
using SachkovTech.Domain.IssueReview.ValueObjects;

namespace SachkovTech.Domain.IssueReview;

public sealed class IssueReview : Entity<IssueReviewId>
{

    // ef core
    private IssueReview(IssueReviewId id) : base(id)
    {
    }

    public IssueReview(IssueReviewId issueReviewId,
        IssueId issueId,
        UserId userId,
        IssueReviewStatus issueReviewStatus,
        DateTime reviewStartedTime,
        DateTime? issueApprovedTime,
        PullRequestLink pullRequestLink)
        : base(issueReviewId)
    {
        IssueId = issueId;
        UserId = userId;
        IssueReviewStatus = issueReviewStatus;
        ReviewStartedTime = reviewStartedTime;
        IssueApprovedTime = issueApprovedTime;
        PullRequestLink = pullRequestLink;
    }

    public IssueId IssueId { get; private set; } 

    public UserId UserId { get; private set; } 
    
    public IssueReviewStatus IssueReviewStatus { get; private set; }

    public UserId? ReviewerId { get; private set; } = null;

    private List<Comment> _comments = [];
    public IReadOnlyList<Comment> Comments => _comments;

    public DateTime ReviewStartedTime { get; private set; }
    public DateTime? IssueTakenTime { get; private set; }
    
    public DateTime? IssueApprovedTime { get; private set; }
    
    public PullRequestLink PullRequestLink { get; private set; }

    public static Result<IssueReview, Error> Create(IssueId issueId,
        UserId userId,
        PullRequestLink pullRequestLink)
    {
        return Result.Success<IssueReview, Error>(new(
            IssueReviewId.NewIssueReviewId(),
            issueId,
            userId,
            IssueReviewStatus.WaitingForReviewer,
            DateTime.UtcNow,
            null,
            pullRequestLink));
    }

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
        if (ReviewerId is null)
        {
            if (comment.UserId != UserId)
            {
                return Errors.General.ValueIsInvalid("comment");
            }
        }
        else
        {
            if ((comment.UserId == UserId || comment.UserId == ReviewerId) == false)
            {
                return Errors.General.ValueIsInvalid("comment");
            }
        }
        
        _comments.Add(comment);

        return UnitResult.Success<Error>();
    }
}