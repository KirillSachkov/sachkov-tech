using System;
using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Application.Commands.StartReview;

public class StartReviewCommandValidator : AbstractValidator<StartReviewCommand>
{
    public StartReviewCommandValidator()
    {
        RuleFor(c => c.IssueReviewId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
    }
}
