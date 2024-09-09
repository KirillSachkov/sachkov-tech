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
        // Arrange
        var title = Title.Create("Test").Value;
        var description = Description.Create("Test").Value;
        var module = new Module(ModuleId.NewModuleId(), title, description);

        var issueId = IssueId.NewIssueId();

        var issue = new Issue(issueId, title, description, LessonId.Empty(), null, null);

        // Act
        var result = module.AddIssue(issue);

        // Assert
        var addedIssueResult = module.GetIssueById(issueId);

        result.IsSuccess.Should().BeTrue();
        addedIssueResult.IsSuccess.Should().BeTrue();
        addedIssueResult.Value.Id.Should().Be(issue.Id);
        addedIssueResult.Value.Position.Should().Be(Position.First);
    }

    [Fact]
    public void Add_Issue_With_Other_Issues_Return_Success_Result()
    {
        // Arrange
        const int issuesCount = 5;
        var module = CreateModuleWithIssues(issuesCount);

        var title = Title.Create("Test").Value;
        var description = Description.Create("Test").Value;

        var issueId = IssueId.NewIssueId();

        var issueToAdd = new Issue(issueId, title, description, LessonId.Empty(), null, null);

        // Act
        var result = module.AddIssue(issueToAdd);

        // Assert
        var addedIssueResult = module.GetIssueById(issueId);

        var serialNumber = Position.Create(issuesCount + 1).Value;

        result.IsSuccess.Should().BeTrue();
        addedIssueResult.IsSuccess.Should().BeTrue();
        addedIssueResult.Value.Id.Should().Be(issueToAdd.Id);
        addedIssueResult.Value.Position.Should().Be(serialNumber);
    }
    
    [Fact]
    public void MoveIssue_ShouldReturnSuccess_WhenIssueAlreadyAtNewPosition()
    {
        // Arrange
        const int issuesCount = 5;

        var positionFrom = Position.Create(2).Value;
        var positionTo = Position.Create(2).Value;
        var module = CreateModuleWithIssues(issuesCount);

        var issueToMove = module.GetIssueByPosition(positionFrom).Value;

        // Act
        var result = module.MoveIssue(issueToMove.Id, positionTo);

        // Assert
        result.IsSuccess.Should().BeTrue();
        positionTo.Should().Be(issueToMove.Position);
    }

    [Fact]
    public void MoveIssue_ShouldMoveOtherIssuesForward_WhenNewPositionIsLower()
    {
        // arrange
        const int issuesCount = 5;

        var positionTo = Position.Create(2).Value;

        var module = CreateModuleWithIssues(issuesCount);

        var firstIssue = module.Issues[0];
        var secondIssue = module.Issues[1];
        var thirdIssue = module.Issues[2];
        var fourthIssue = module.Issues[3];
        var fifthIssue = module.Issues[4];

        // Act
        var result = module.MoveIssue(fourthIssue.Id, positionTo);

        // Assert

        result.IsSuccess.Should().Be(true);
        firstIssue.Position.Value.Should().Be(1);
        secondIssue.Position.Value.Should().Be(3);
        thirdIssue.Position.Value.Should().Be(4);
        fourthIssue.Position.Should().Be(positionTo);
        fifthIssue.Position.Value.Should().Be(5);
    }
    
    [Fact]
    public void MoveIssue_ShouldMoveToLastPosition_WhenNewPositionIsMoreNumberOfIssues()
    {
        // arrange
        const int issuesCount = 5;

        var positionTo = Position.Create(10).Value;

        var module = CreateModuleWithIssues(issuesCount);

        var firstIssue = module.Issues[0];
        var secondIssue = module.Issues[1];
        var thirdIssue = module.Issues[2];
        var fourthIssue = module.Issues[3];
        var fifthIssue = module.Issues[4];

        // Act
        var result = module.MoveIssue(secondIssue.Id, positionTo);

        result.IsSuccess.Should().Be(true);
        firstIssue.Position.Value.Should().Be(1);
        secondIssue.Position.Value.Should().Be(4);
        thirdIssue.Position.Value.Should().Be(2);
        fourthIssue.Position.Value.Should().Be(3);
        fifthIssue.Position.Value.Should().Be(5);
    }

    [Fact]
    public void MoveIssue_ShouldMoveOtherIssuesBack_WhenNewPositionIsBigger()
    {
        // arrange
        const int issuesCount = 5;

        var positionTo = Position.Create(4).Value;

        var module = CreateModuleWithIssues(issuesCount);

        var firstIssue = module.Issues[0];
        var secondIssue = module.Issues[1];
        var thirdIssue = module.Issues[2];
        var fourthIssue = module.Issues[3];
        var fifthIssue = module.Issues[4];

        // Act
        var result = module.MoveIssue(secondIssue.Id, positionTo);

        result.IsSuccess.Should().Be(true);
        firstIssue.Position.Value.Should().Be(1);
        secondIssue.Position.Value.Should().Be(4);
        thirdIssue.Position.Value.Should().Be(2);
        fourthIssue.Position.Value.Should().Be(3);
        fifthIssue.Position.Value.Should().Be(5);
    }

    [Fact]
    public void MoveIssue_ShouldMoveOtherIssuesForward_WhenNewPositionIsFirst()
    {
        // arrange
        const int issuesCount = 5;

        var positionTo = Position.Create(1).Value;

        var module = CreateModuleWithIssues(issuesCount);

        var firstIssue = module.Issues[0];
        var secondIssue = module.Issues[1];
        var thirdIssue = module.Issues[2];
        var fourthIssue = module.Issues[3];
        var fifthIssue = module.Issues[4];

        // Act
        var result = module.MoveIssue(fifthIssue.Id, positionTo);

        result.IsSuccess.Should().Be(true);
        firstIssue.Position.Value.Should().Be(2);
        secondIssue.Position.Value.Should().Be(3);
        thirdIssue.Position.Value.Should().Be(4);
        fourthIssue.Position.Value.Should().Be(5);
        fifthIssue.Position.Value.Should().Be(1);
    }

    [Fact]
    public void MoveIssue_ShouldMoveOtherIssuesBack_WhenNewPositionIsLast()
    {
        // arrange
        const int issuesCount = 5;

        var positionTo = Position.Create(5).Value;

        var module = CreateModuleWithIssues(issuesCount);

        var firstIssue = module.Issues[0];
        var secondIssue = module.Issues[1];
        var thirdIssue = module.Issues[2];
        var fourthIssue = module.Issues[3];
        var fifthIssue = module.Issues[4];

        // Act
        var result = module.MoveIssue(firstIssue.Id, positionTo);

        result.IsSuccess.Should().Be(true);
        firstIssue.Position.Value.Should().Be(5);
        secondIssue.Position.Value.Should().Be(1);
        thirdIssue.Position.Value.Should().Be(2);
        fourthIssue.Position.Value.Should().Be(3);
        fifthIssue.Position.Value.Should().Be(4);
    }

    private Module CreateModuleWithIssues(int issuesCount)
    {
        var title = Title.Create("Test").Value;
        var description = Description.Create("Test").Value;
        var module = new Module(ModuleId.NewModuleId(), title, description);

        for (var i = 0; i < issuesCount; i++)
        {
            var issue = new Issue(IssueId.NewIssueId(), title, description, LessonId.Empty(), null, null);
            module.AddIssue(issue);
        }

        return module;
    }
}