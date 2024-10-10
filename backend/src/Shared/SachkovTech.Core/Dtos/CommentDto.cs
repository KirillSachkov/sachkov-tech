namespace SachkovTech.Core.Dtos;

public record CommentDto(Guid Id,
    Guid UserId,
    string Message, 
    DateTime CreatedAt,
    //Для фильтрации
    Guid IssueReviewId);