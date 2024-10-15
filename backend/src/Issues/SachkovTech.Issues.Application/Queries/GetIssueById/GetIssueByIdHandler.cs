using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Contracts;
using SachkovTech.Issues.Contracts.Responses;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Queries.GetIssueById;

public class GetIssueByIdHandler : IQueryHandlerWithResult<IssueResponse, GetIssueByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IFilesContracts _filesContracts;

    public GetIssueByIdHandler(IReadDbContext readDbContext, IFilesContracts filesContracts)
    {
        _readDbContext = readDbContext;
        _filesContracts = filesContracts;
    }

    public async Task<Result<IssueResponse, ErrorList>> Handle(
        GetIssueByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var issueDto = await _readDbContext.Issues
            .SingleOrDefaultAsync(i => i.Id == query.IssueId, cancellationToken);

        if (issueDto is null)
            return Errors.General.NotFound(query.IssueId).ToErrorList();

        var fileLinks = await _filesContracts.GetLinkFiles(issueDto.Files);

        var response = new IssueResponse(
            issueDto.Id,
            issueDto.ModuleId,
            issueDto.Title,
            issueDto.Description,
            issueDto.Position,
            issueDto.LessonId,
            fileLinks.Select(f => new FileResponse(f.FileId, f.Link)).ToArray());

        return response;
    }
}