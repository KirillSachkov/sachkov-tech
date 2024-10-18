using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.IssueSolving.Application;
using SachkovTech.IssueSolving.Domain.Entities;
using SachkovTech.IssueSolving.Infrastructure.DbContexts;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueSolving.Infrastructure.Repositories;

public class UserIssueRepository : IUserIssueRepository
{
    private readonly IssueSolvingWriteDbContext _dbContext;

    public UserIssueRepository(IssueSolvingWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(UserIssue userIssue, CancellationToken cancellationToken = default)
    {
        await _dbContext.UserIssues.AddAsync(userIssue, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return userIssue.Id;
    }

    public async Task<Result<UserIssue, Error>> GetUserIssueById(UserIssueId userIssueId,
        CancellationToken cancellationToken = default)
    {
        var userIssue =
            await _dbContext.UserIssues.SingleOrDefaultAsync(ui => ui.Id == userIssueId, cancellationToken);

        if (userIssue is null)
        {
            return Errors.General.NotFound();
        }

        return userIssue;
    }
}