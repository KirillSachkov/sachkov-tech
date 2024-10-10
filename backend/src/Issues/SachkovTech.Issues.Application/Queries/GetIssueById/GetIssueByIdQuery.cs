using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Queries.GetIssueById;

public record GetIssueByIdQuery(Guid IssueId) : IQuery;