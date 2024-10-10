using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Queries.GetIssueByPosition;

public record GetIssueByPositionQuery(int Position) : IQuery;