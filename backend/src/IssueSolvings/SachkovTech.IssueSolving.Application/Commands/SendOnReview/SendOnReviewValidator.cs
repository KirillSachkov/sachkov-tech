using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.IssueSolving.Domain.ValueObjects;

namespace SachkovTech.IssueSolving.Application.Commands.SendOnReview;

public class SendOnReviewValidator : AbstractValidator<SendOnReviewCommand>
{
    public SendOnReviewValidator()
    {
        RuleFor(s => s.PullRequestUrl).MustBeValueObject(PullRequestUrl.Create);
    }
}