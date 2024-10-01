using CSharpFunctionalExtensions;
using SachkovTech.Domain.IssueReview.Entities;
using SachkovTech.Domain.IssueReview.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Domain.IssueReview;

public sealed class IssueReview : CSharpFunctionalExtensions.Entity<IssueReviewId>
{

    // ef core
    private IssueReview(IssueReviewId id) : base(id)
    {
    }

    public IssueReview(IssueReviewId issueReviewId,
        IssueId issueId,
        UserId userId,
        IssueReviewStatusInfo issueReviewStatus,
        ReviewerId? reviewerId,
        IReadOnlyList<Comment> comments,
        DateTime issueTakenTime,
        DateTime issueCreatedTime,
        PullRequestLink pullRequestLink)
        : base(issueReviewId)
    {
        IssueId = issueId;
        UserId = userId;
        IssueReviewStatus = issueReviewStatus;
        ReviewerId = reviewerId;
        Comments = comments;
        IssueTakenTime = issueTakenTime;
        IssueCreatedTime = issueCreatedTime;
        PullRequestLink = pullRequestLink;
    }

    public IssueId IssueId { get; private set; } 

    public UserId UserId { get; private set; } 
    
    public IssueReviewStatusInfo IssueReviewStatus { get; private set; }

    public ReviewerId? ReviewerId { get; private set; } = null;
    
    public IReadOnlyList<Comment> Comments { get; private set; }

    public DateTime IssueTakenTime { get; private set; } = default;
    
    public DateTime IssueCreatedTime { get; private set; }
    
    public PullRequestLink PullRequestLink { get; private set; }

    public static Result<IssueReview, Error> Create(IssueId issueId,
        UserId userId,
        IssueReviewStatusInfo issueReviewStatus,
        ReviewerId? reviewerId,
        IReadOnlyList<Comment> comments,
        DateTime issueTakenTime,
        DateTime issueCreatedTime,
        PullRequestLink pullRequestLink)
    {
        return Result.Success<IssueReview, Error>(new(
            IssueReviewId.NewIssueReviewId(),
            issueId,
            userId,
            issueReviewStatus,
            reviewerId,
            comments,
            issueTakenTime,
            issueCreatedTime,
            pullRequestLink));
    }

    public void SetIssueOnReview(ReviewerId? reviewerId)
    {
        ReviewerId = reviewerId;
        IssueReviewStatus = IssueReviewStatusInfo.Create(2).Value;

        if (IssueTakenTime == default)
        {
            IssueTakenTime = DateTime.UtcNow;
        }
    }
    
    public void AskForRevision()
    {
        IssueReviewStatus = IssueReviewStatusInfo.Create(4).Value;
    }
    
    public void AcceptIssue()
    {
        IssueReviewStatus = IssueReviewStatusInfo.Create(3).Value;
    }
    
    public UnitResult<Error> CreateComment(Comment comment)
    {
        //TODO: упростить
        if (ReviewerId is null)
        {
            if (comment.CommentatorId.Value != UserId.Value)
            {
                return Errors.General.ValueIsInvalid("comment");
            }
        }
        else
        {
            if (comment.CommentatorId.Value != UserId.Value &&
                comment.CommentatorId.Value != ReviewerId.Value)
            {
                return Errors.General.ValueIsInvalid("comment");
            }
        }

        var newComments = new List<Comment>(capacity: Comments.Count+1);

        foreach (var commentTemp in Comments)
        {
            newComments.Add(commentTemp);
        }
        
        newComments.Add(comment);

        Comments = newComments;

        return UnitResult.Success<Error>();
    }
}