using CSharpFunctionalExtensions;
using SachkovTech.IssueSolving.Domain.Entities;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueSolving.Application;

public interface IUserIssueRepository
{
    Task<Guid> Add(UserIssue userIssue, CancellationToken cancellationToken = default);

    Task<Result<UserIssue, Error>> GetUserIssueById(UserIssueId userIssueId,
            CancellationToken cancellationToken = default);
}
