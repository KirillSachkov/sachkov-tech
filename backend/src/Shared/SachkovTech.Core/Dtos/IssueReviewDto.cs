﻿namespace SachkovTech.Core.Dtos;

public class IssueReviewDto
{
    public Guid Id { get; set; }
    
    public Guid UserIssueId { get; init; }

    public Guid UserId { get; init; }

    public Guid? ReviewerId { get; init; }

    public string IssueReviewStatus { get; init; } = string.Empty;

    public IEnumerable<CommentDto> Comments { get; init; } = [];

    public DateTime ReviewStartedTime { get; init; }

    public DateTime? IssueTakenTime { get; init; }

    public DateTime? IssueApprovedTime { get; init; }

    public string PullRequestLink { get; init; } = string.Empty;
}