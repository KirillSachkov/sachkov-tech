using FluentAssertions;
using SachkovTech.Domain.IssueManagement;
using SachkovTech.Domain.IssueManagement.Entities;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Domain.UnitTests;

public class ModuleTests
{
    [Fact]
    public void Add_Issue_With_Empty_Issues_Return_Success_Result()
    {
        // arrange
        var title = Title.Create("Test").Value;
        var description = Description.Create("Test").Value;
        var module = new Module(ModuleId.NewModuleId(), title, description);

        var issueId = IssueId.NewIssueId();

        var issue = new Issue(issueId, title, description, LessonId.Empty(), null, null);

        // act
        var result = module.AddIssue(issue);

        // assert
        var addedIssueResult = module.GetIssueById(issueId);

        result.IsSuccess.Should().BeTrue();
        addedIssueResult.IsSuccess.Should().BeTrue();
        addedIssueResult.Value.Id.Should().Be(issue.Id);
        addedIssueResult.Value.Position.Should().Be(Position.First);
    }

    [Fact]
    public void Add_Issue_With_Other_Issues_Return_Success_Result()
    {
        // arrange
        const int issuesCount = 5;
        
        var title = Title.Create("Test").Value;
        var description = Description.Create("Test").Value;
        var module = new Module(ModuleId.NewModuleId(), title, description);

        var issues = Enumerable.Range(1, issuesCount).Select(_ =>
            new Issue(IssueId.NewIssueId(), title, description, LessonId.Empty(), null, null));
        
        var issueId = IssueId.NewIssueId();

        var issueToAdd = new Issue(issueId, title, description, LessonId.Empty(), null, null);

        foreach (var issue in issues)
            module.AddIssue(issue);
        
         // act
        var result = module.AddIssue(issueToAdd);

        // assert
        var addedIssueResult = module.GetIssueById(issueId);

        var serialNumber = Position.Create(issuesCount + 1).Value;

        result.IsSuccess.Should().BeTrue();
        addedIssueResult.IsSuccess.Should().BeTrue();
        addedIssueResult.Value.Id.Should().Be(issueToAdd.Id);
        addedIssueResult.Value.Position.Should().Be(serialNumber);
    }
}