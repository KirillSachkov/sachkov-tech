using System;
using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesReviews.Application.Commands.StartReview;

public record StartReviewCommand(
    Guid IssueReviewId,
    Guid ReviewerId) : ICommand;