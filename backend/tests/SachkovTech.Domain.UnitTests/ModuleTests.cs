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
        var title = Title.Create("Test").Value;
        var description = Description.Create("Test").Value;
        
        // arrange
        var module = CreateModuleWithIssues(0);

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
        
        var module = CreateModuleWithIssues(issuesCount);
        
        var issueId = IssueId.NewIssueId();

        var issueToAdd = new Issue(issueId, title, description, LessonId.Empty(), null, null);
        
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

    [Fact]
    public void Move_Issue_Should_Not_Move_When_Issue_Already_At_New_Position()
    {
        // arrange
        const int issuesCount = 5;
        
        var module = CreateModuleWithIssues(issuesCount);

        var secondPosition = Position.Create(2).Value;
        
        var firstIssue = module.Issues[0];
        var secondIssue = module.Issues[1];
        var thirdIssue = module.Issues[2];
        var fourthIssue = module.Issues[3];
        var fifthIssue = module.Issues[4];
        
        // act
        var result = module.MoveIssue(secondIssue, secondPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstIssue.Position.Should().Be(Position.Create(1).Value);
        secondIssue.Position.Should().Be(Position.Create(2).Value);
        thirdIssue.Position.Should().Be(Position.Create(3).Value);
        fourthIssue.Position.Should().Be(Position.Create(4).Value);
        fifthIssue.Position.Should().Be(Position.Create(5).Value);
    }
    
    [Fact]
    public void Move_Issue_Should_Move_Other_Issues_Forward_When_New_Position_Is_Lower()
    {
        // arrange
        const int issuesCount = 5;
        
        var module = CreateModuleWithIssues(issuesCount);

        var secondPosition = Position.Create(2).Value;
        
        var firstIssue = module.Issues[0];
        var secondIssue = module.Issues[1];
        var thirdIssue = module.Issues[2];
        var fourthIssue = module.Issues[3];
        var fifthIssue = module.Issues[4];
        
        // act
        var result = module.MoveIssue(fourthIssue, secondPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstIssue.Position.Should().Be(Position.Create(1).Value);
        secondIssue.Position.Should().Be(Position.Create(3).Value);
        thirdIssue.Position.Should().Be(Position.Create(4).Value);
        fourthIssue.Position.Should().Be(Position.Create(2).Value);
        fifthIssue.Position.Should().Be(Position.Create(5).Value);
    }
    
    [Fact]
    public void Move_Issue_Should_Move_Other_Issues_Back_When_New_Position_Is_Grater()
    {
        // arrange
        const int issuesCount = 5;
        
        var module = CreateModuleWithIssues(issuesCount);

        var fourthPosition = Position.Create(4).Value;
        
        var firstIssue = module.Issues[0];
        var secondIssue = module.Issues[1];
        var thirdIssue = module.Issues[2];
        var fourthIssue = module.Issues[3];
        var fifthIssue = module.Issues[4];
        
        // act
        var result = module.MoveIssue(secondIssue, fourthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstIssue.Position.Should().Be(Position.Create(1).Value);
        secondIssue.Position.Should().Be(Position.Create(4).Value);
        thirdIssue.Position.Should().Be(Position.Create(2).Value);
        fourthIssue.Position.Should().Be(Position.Create(3).Value);
        fifthIssue.Position.Should().Be(Position.Create(5).Value);
    }
    
    [Fact]
    public void Move_Issue_Should_Move_Other_Issues_Forward_When_New_Position_Is_First()
    {
        // arrange
        const int issuesCount = 5;
        
        var module = CreateModuleWithIssues(issuesCount);

        var firstPosition = Position.Create(1).Value;
        
        var firstIssue = module.Issues[0];
        var secondIssue = module.Issues[1];
        var thirdIssue = module.Issues[2];
        var fourthIssue = module.Issues[3];
        var fifthIssue = module.Issues[4];
        
        // act
        var result = module.MoveIssue(fifthIssue, firstPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstIssue.Position.Should().Be(Position.Create(2).Value);
        secondIssue.Position.Should().Be(Position.Create(3).Value);
        thirdIssue.Position.Should().Be(Position.Create(4).Value);
        fourthIssue.Position.Should().Be(Position.Create(5).Value);
        fifthIssue.Position.Should().Be(Position.Create(1).Value);
    }
    
    [Fact]
    public void Move_Issue_Should_Move_Other_Issues_Back_When_New_Position_Is_Last()
    {
        // arrange
        const int issuesCount = 5;
        
        var module = CreateModuleWithIssues(issuesCount);

        var fifthPosition = Position.Create(5).Value;
        
        var firstIssue = module.Issues[0];
        var secondIssue = module.Issues[1];
        var thirdIssue = module.Issues[2];
        var fourthIssue = module.Issues[3];
        var fifthIssue = module.Issues[4];
        
        // act
        var result = module.MoveIssue(firstIssue, fifthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstIssue.Position.Should().Be(Position.Create(5).Value);
        secondIssue.Position.Should().Be(Position.Create(1).Value);
        thirdIssue.Position.Should().Be(Position.Create(2).Value);
        fourthIssue.Position.Should().Be(Position.Create(3).Value);
        fifthIssue.Position.Should().Be(Position.Create(4).Value);
    }
    
    [Fact]
    public void Check_FilePath_ToBeValid_WhenItsFullPath()
    {
        // arrange
        string fullPath1 = "bobmaster.jpg";
        string fullPath2 = "dasdadad.mp4";
        string fullPath3 = Guid.NewGuid().ToString() + ".mkv";
        string fullPath4 = Guid.NewGuid().ToString() + ".jfif";
        string fullPath5 = Guid.NewGuid().ToString() + ".flac";

        var guidPath = Guid.Parse("e3147aa9-97be-4fee-b50d-2e73965a16e9");
        string fullPath6 = guidPath + ".raw";
        
        // act
        var filePath1Res = FilePath.Create(fullPath1);
        var filePath2Res = FilePath.Create(fullPath2);
        var filePath3Res = FilePath.Create(fullPath3);
        var filePath4Res = FilePath.Create(fullPath4);
        var filePath5Res = FilePath.Create(fullPath5);
        var filePath6Res = FilePath.Create(fullPath6);

        // assert
        filePath1Res.IsSuccess.Should().BeTrue();
        filePath2Res.IsSuccess.Should().BeFalse();
        filePath3Res.IsSuccess.Should().BeFalse();
        filePath4Res.IsSuccess.Should().BeTrue();
        filePath5Res.IsSuccess.Should().BeFalse();
        filePath6Res.IsSuccess.Should().BeTrue();
        
        filePath6Res.Value.Path.Should().Be("e3147aa9-97be-4fee-b50d-2e73965a16e9.raw");
    }
    
    [Fact]
    public void Check_FilePath_ToBeValid_WhenItsPathAndExtension()
    {
        // arrange
        Guid path = Guid.NewGuid();
        string extension1 = ".tiff";
        string extension2 = ".rtf";
        string extension3 = ".bmp";
        string extension4 = ".docx";
        string extension5 = ".exe";

        Guid path6 = Guid.Parse("e3147aa9-97be-4fee-b50d-2e73965a16e9");
        string extension6 = ".txt";
        
        // act
        var path1Res = FilePath.Create(path, extension1);
        var path2Res = FilePath.Create(path, extension2);
        var path3Res = FilePath.Create(path, extension3);
        var path4Res = FilePath.Create(path, extension4);
        var path5Res = FilePath.Create(path, extension5);
        var path6Res = FilePath.Create(path6, extension6);

        // assert
        path1Res.IsSuccess.Should().BeTrue();
        path2Res.IsSuccess.Should().BeTrue();
        path3Res.IsSuccess.Should().BeTrue();
        path4Res.IsSuccess.Should().BeTrue();
        path5Res.IsSuccess.Should().BeFalse();
        path6Res.IsSuccess.Should().BeTrue();
        
        path6Res.Value.Path.Should().Be("e3147aa9-97be-4fee-b50d-2e73965a16e9.txt");
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