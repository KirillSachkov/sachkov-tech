using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.IssuesReviews.Application.Commands.Create;

public class CreateIssueReviewValidator : AbstractValidator<CreateIssueReviewCommand>
{
    public CreateIssueReviewValidator()
    {
        RuleFor(c => c.PullRequestUrl).MustBeValueObject(PullRequestUrl.Create);
    }
}