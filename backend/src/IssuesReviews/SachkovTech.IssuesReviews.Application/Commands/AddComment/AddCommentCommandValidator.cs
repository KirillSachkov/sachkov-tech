using FluentValidation;
using SachkovTech.Core.Validation;
using SachkovTech.IssuesReviews.Domain.ValueObjects;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Application.Commands.AddComment;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(c => c.IssueReviewId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("id"));
        RuleFor(c => c.Message).MustBeValueObject(Message.Create);
    }
}