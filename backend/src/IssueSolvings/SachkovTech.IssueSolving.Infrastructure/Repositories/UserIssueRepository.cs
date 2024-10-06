using SachkovTech.IssueSolving.Application;
using SachkovTech.IssueSolving.Domain.Entities;
using SachkovTech.IssueSolving.Infrastructure.DbContexts;

namespace SachkovTech.IssueSolving.Infrastructure.Repositories
{
    public class UserIssueRepository : IUserIssueRepository
    {
        private readonly WriteDbContext _dbContext;

        public UserIssueRepository(WriteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Add(UserIssue userIssue, CancellationToken cancellationToken = default)
        {
            await _dbContext.UserIssues.AddAsync(userIssue, cancellationToken);
            return userIssue.Id;
        }
    }
}
