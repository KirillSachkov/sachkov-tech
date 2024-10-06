using CSharpFunctionalExtensions;
using SachkovTech.Core.Dtos;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Contracts;

public interface IIssuesContract
{
    Task<Result<IssueDto, ErrorList>> GetIssueById(
        Guid issueId, CancellationToken cancellationToken = default);

    Task<Result<IssueDto, ErrorList>> GetIssueByPosition(
        int position, CancellationToken cancellationToken = default);
}