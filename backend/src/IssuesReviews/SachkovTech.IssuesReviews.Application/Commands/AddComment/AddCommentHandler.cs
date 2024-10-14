using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.IssuesReviews.Domain.Entities;
using SachkovTech.IssuesReviews.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Application.Commands.AddComment;

public class AddCommentHandler : ICommandHandler<Guid, AddCommentCommand>
{
    private readonly IIssueReviewRepository _issueReviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddCommentCommand> _validator;
    private readonly ILogger<AddCommentHandler> _logger;

    public AddCommentHandler(
        IIssueReviewRepository issueReviewRepository,
        [FromKeyedServices(Modules.IssuesReviews)] IUnitOfWork unitOfWork,
        IValidator<AddCommentCommand> validator,
        ILogger<AddCommentHandler> logger)
    {
        _issueReviewRepository = issueReviewRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddCommentCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var issueReviewResult = await _issueReviewRepository
            .GetById(IssueReviewId.Create(command.IssueReviewId), cancellationToken);

        if (issueReviewResult.IsFailure)
            return issueReviewResult.Error.ToErrorList();

        var message = Message.Create(command.Message).Value;

        var comment = Comment.Create(UserId.Create(command.UserId), message);

        //Хоть и проверка всегда вернет false на будущее если валидация внутри Comment будет присутствовать
        //оставим это поле.
        if (comment.IsFailure)
            return comment.Error.ToErrorList();

        var addCommentResult = issueReviewResult.Value.AddComment(comment.Value);

        if (addCommentResult.IsFailure)
            return addCommentResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Comment {commentId} was created in issueReview {issueReviewId}",
            comment.Value.Id.Value,
            command.IssueReviewId);

        return comment.Value.Id.Value;
    }
}