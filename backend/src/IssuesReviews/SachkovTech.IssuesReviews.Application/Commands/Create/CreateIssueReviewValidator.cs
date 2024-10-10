using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.IssueSolving.Domain.ValueObjects;

namespace SachkovTech.IssuesReviews.Application.Commands.Create;

public class CreateIssueReviewValidator : AbstractValidator<CreateIssueReviewCommand>
{
    public CreateIssueReviewValidator()
    {
        RuleFor(c => c.PullRequestUrl).MustBeValueObject(PullRequestUrl.Create);
    }
}