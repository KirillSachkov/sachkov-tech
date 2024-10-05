using SachkovTech.IssueSolving.Application;
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
    }
}
