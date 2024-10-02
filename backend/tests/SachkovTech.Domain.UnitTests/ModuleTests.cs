using FluentAssertions;
using SachkovTech.Domain.IssueManagement;
using SachkovTech.Domain.IssueManagement.Entities;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.IssueReview.Entities;
using SachkovTech.Domain.IssueReview.Other;
using SachkovTech.Domain.IssueReview.ValueObjects;
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
    
    [Fact]
    public void Issue_StartReview_OnlyOnce()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var reviewerId = UserId.NewUserId();

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        // act
        newIssueReview.StartReview(reviewerId);

        // assert
        oldIssueReview.ReviewerId.Should().BeNull();
        newIssueReview.ReviewerId.Should().Be(reviewerId);
        oldIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.WaitingForReviewer);
        newIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.OnReview);
        oldIssueReview.IssueTakenTime.Should().BeNull();
        newIssueReview.IssueTakenTime.Should().NotBeNull();
    }
    
    [Fact]
    public void Issue_StartReview_MoreThanOnce()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var reviewerId = UserId.NewUserId();

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        // act
        newIssueReview.StartReview(reviewerId);
        newIssueReview.StartReview(reviewerId);
        newIssueReview.StartReview(reviewerId);

        // assert
        oldIssueReview.ReviewerId.Should().BeNull();
        newIssueReview.ReviewerId.Should().Be(reviewerId);
        oldIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.WaitingForReviewer);
        newIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.OnReview);
        oldIssueReview.IssueTakenTime.Should().BeNull();
        newIssueReview.IssueTakenTime.Should().NotBeNull();
    }
    
    [Fact]
    public void Issue_SendIssueForRevision()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var reviewerId = UserId.NewUserId();

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        // act
        oldIssueReview.StartReview(reviewerId);
        newIssueReview.StartReview(reviewerId);
        newIssueReview.SendIssueForRevision();

        // assert
        oldIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.OnReview);
        newIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.AskedForRevision);
    }
    
    [Fact]
    public void Issue_ApproveIssue()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var reviewerId = UserId.NewUserId();

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        // act
        oldIssueReview.StartReview(reviewerId);
        newIssueReview.StartReview(reviewerId);
        newIssueReview.Approve();

        // assert
        oldIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.OnReview);
        newIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatus.Accepted);
    }
    
    [Fact]
    public void IssueReview_ValueObjectValidation_PullRequestLink()
    {
        // arrange & act
        var pullRequestLink1 = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4");
        var pullRequestLink2 = PullRequestLink
            .Create(@"https://github.com/BOBBOMBA/some-super-tech/pull/43");
        var pullRequestLink3 = PullRequestLink
            .Create(@"https://www.youtube.com/watch?v=8VOuxijh9_s&list=PLcvhF2Wqh7DNVy1OCUpG3i5lyxyBWhGZ8");
        var pullRequestLink4 = PullRequestLink
            .Create(@"https://www.google.com/search?q=%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4%D1%87%D0%B8%D0%BA&oq
=&gs_lcrp=EgZjaHJvbWUqBggBEEUYOzIGCAAQRRg5MgYIARBFGDsyBggCEEUYO9IBBzgwMGowajeoAgCwAgA&sourceid=chrome&ie=UTF-8");
        var pullRequestLink5 = PullRequestLink
            .Create(@"https://github.com/aznoran/PetHouse/pull/32");
        
        // assert
        pullRequestLink1.IsSuccess.Should().BeTrue();
        pullRequestLink2.IsSuccess.Should().BeTrue();
        pullRequestLink3.IsSuccess.Should().BeFalse();
        pullRequestLink4.IsSuccess.Should().BeFalse();
        pullRequestLink5.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public void IssueReview_ValueObjectValidation_Message()
    {
        // arrange & act
        var message1 = Message.Create("small text");
        string bigText = "b";
        for (int i = 0; i < 2000; ++i)
        {
            bigText += "i";
        }
        var message2 = Message.Create(bigText);
        
        // assert
        message1.IsSuccess.Should().BeTrue();
        message2.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public void Issue_AddComment_WhenUserAdds()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueReviewStatus = IssueReviewStatus.OnReview;

        var newMessage = Message.Create("something").Value;

        var newComment = Comment.Create(
            userId,
            newMessage);

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;
        

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        // act
        var createCommentRes = newIssueReview.AddComment(newComment.Value);

        // assert
        createCommentRes.IsSuccess.Should().BeTrue();
        oldIssueReview.Comments.Count.Should().Be(0);
        newIssueReview.Comments[0].Should().Be(newComment.Value);
        newIssueReview.Comments[0].UserId.Should().Be(userId);
    }
    
    [Fact]
    public void Issue_AddComment_WhenReviewerAdds()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueReviewStatus = IssueReviewStatus.OnReview;
        var reviewerId = UserId.NewUserId();

        var newMessage = Message.Create("something").Value;

        var newComment = Comment.Create(
            reviewerId,
            newMessage);

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        // act
        oldIssueReview.StartReview(reviewerId);
        newIssueReview.StartReview(reviewerId);
        var createCommentRes = newIssueReview.AddComment(newComment.Value);

        // assert
        createCommentRes.IsSuccess.Should().BeTrue();
        oldIssueReview.Comments.Count.Should().Be(0);
        newIssueReview.Comments[0].Should().Be(newComment.Value);
        newIssueReview.Comments[0].UserId.Should().Be(reviewerId);
    }
    
    [Fact]
    public void Issue_AddComment_WhenCommentatorIsInvalid()
    {
        // arrange
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueReviewStatus = IssueReviewStatus.OnReview;
        var reviewerId = UserId.NewUserId();

        var newMessage = Message.Create("something").Value;

        var newComment = Comment.Create(
            UserId.Create(Guid.NewGuid()),
            newMessage).Value;

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            pullRequestLink).Value;
        
        // act
        oldIssueReview.StartReview(reviewerId);
        newIssueReview.StartReview(reviewerId);
        var createCommentRes = newIssueReview.AddComment(newComment);

        // assert
        createCommentRes.IsFailure.Should().BeTrue();
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