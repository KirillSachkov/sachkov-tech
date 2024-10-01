using CSharpFunctionalExtensions;
using SachkovTech.Domain.IssueReview.Other;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.IssueReview.ValueObjects;

public record IssueReviewStatusInfo
{
    private IssueReviewStatusInfo(IssueReviewStatus value)
    {
        Value = value;
    }
    
    public IssueReviewStatus Value { get; }
    
    public static Result<IssueReviewStatusInfo, Error> Create(int value)
    {
        IssueReviewStatus issueReviewStatus = (IssueReviewStatus)value;

        if (!Enum.IsDefined(typeof(IssueReviewStatus), issueReviewStatus))
        {
            return Errors.General.ValueIsInvalid("pet-status");
        }
        
        return new IssueReviewStatusInfo(issueReviewStatus);
    }
}