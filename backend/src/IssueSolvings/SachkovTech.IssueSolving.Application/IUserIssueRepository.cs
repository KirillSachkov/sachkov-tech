using SachkovTech.IssueSolving.Domain.Entities;

namespace SachkovTech.IssueSolving.Application
{
    public interface IUserIssueRepository
    {
        Task<Guid> Add(UserIssue userIssue, CancellationToken cancellationToken = default);
    }
}
