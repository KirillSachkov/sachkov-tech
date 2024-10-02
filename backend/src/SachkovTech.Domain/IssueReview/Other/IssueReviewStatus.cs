namespace SachkovTech.Domain.IssueReview.Other;

public enum IssueReviewStatus
{
    WaitingForReviewer = 1,
    OnReview = 2,
    Accepted = 3,
    AskedForRevision = 4
}