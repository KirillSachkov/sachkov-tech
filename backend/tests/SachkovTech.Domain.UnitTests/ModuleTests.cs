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
    public void Set_Issue_OnReview_OnlyOnce()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueReviewStatus = IssueReviewStatusInfo.Create(1).Value;
        var reviewerId = ReviewerId.NewReviewerId();

        var message = Message.Create("test").Value;
        var comments = new List<Comment>()
        {
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(reviewerId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value
        };

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            comments,
            default,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            comments,
            default,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        // act
        newIssueReview.SetIssueOnReview(reviewerId);

        // assert
        oldIssueReview.ReviewerId.Should().BeNull();
        newIssueReview.ReviewerId.Should().Be(reviewerId);
        oldIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatusInfo.Create(1).Value);
        newIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatusInfo.Create(2).Value);
        oldIssueReview.IssueTakenTime.Should().Be(default);
        newIssueReview.IssueTakenTime.Should().NotBe(default);
    }
    
    [Fact]
    public void Set_Issue_OnReview_MoreThanOnce()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueReviewStatus = IssueReviewStatusInfo.Create(1).Value;
        var reviewerId = ReviewerId.NewReviewerId();

        var message = Message.Create("test").Value;
        var comments = new List<Comment>()
        {
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(reviewerId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value
        };

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;
        
        var issueTakenTime = DateTime.UtcNow;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            comments,
            default,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            comments,
            issueTakenTime,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        // act
        newIssueReview.SetIssueOnReview(reviewerId);
        newIssueReview.SetIssueOnReview(reviewerId);
        newIssueReview.SetIssueOnReview(reviewerId);

        // assert
        oldIssueReview.ReviewerId.Should().BeNull();
        newIssueReview.ReviewerId.Should().Be(reviewerId);
        oldIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatusInfo.Create(1).Value);
        newIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatusInfo.Create(2).Value);
        oldIssueReview.IssueTakenTime.Should().Be(default);
        newIssueReview.IssueTakenTime.Should().Be(issueTakenTime);
    }
    
    [Fact]
    public void Issue_AskForRevision()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueReviewStatus = IssueReviewStatusInfo.Create(1).Value;
        var reviewerId = ReviewerId.NewReviewerId();

        var message = Message.Create("test").Value;
        var comments = new List<Comment>()
        {
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(reviewerId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value
        };

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;
        
        var issueTakenTime = DateTime.UtcNow;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            comments,
            default,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            comments,
            issueTakenTime,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        // act
        newIssueReview.AskForRevision();

        // assert
        oldIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatusInfo.Create(1).Value);
        newIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatusInfo.Create(4).Value);
    }
    
    [Fact]
    public void Issue_AcceptIssue()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueReviewStatus = IssueReviewStatusInfo.Create(1).Value;
        var reviewerId = ReviewerId.NewReviewerId();

        var message = Message.Create("test").Value;
        var comments = new List<Comment>()
        {
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(reviewerId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value
        };

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;
        
        var issueTakenTime = DateTime.UtcNow;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            comments,
            default,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            comments,
            issueTakenTime,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        // act
        newIssueReview.AcceptIssue();

        // assert
        oldIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatusInfo.Create(1).Value);
        newIssueReview.IssueReviewStatus.Should().Be(IssueReviewStatusInfo.Create(3).Value);
    }
    
    //TODO: проверка VO (Pullreqlink , message, statusinfo)
    [Fact]
    public void IssueReview_ValueObjectValidation_IssueReviewStatusInfo()
    {
        // arrange & act
        var reviewStatus1 = IssueReviewStatusInfo.Create(0);
        var reviewStatus2 = IssueReviewStatusInfo.Create(1);
        var reviewStatus3 = IssueReviewStatusInfo.Create(2);
        var reviewStatus4 = IssueReviewStatusInfo.Create(3);
        var reviewStatus5 = IssueReviewStatusInfo.Create(4);
        var reviewStatus6 = IssueReviewStatusInfo.Create(5);
        var reviewStatus7 = IssueReviewStatusInfo.Create(555);
        
        // assert
        reviewStatus1.IsSuccess.Should().BeFalse();
        reviewStatus2.IsSuccess.Should().BeTrue();
        reviewStatus3.IsSuccess.Should().BeTrue();
        reviewStatus4.IsSuccess.Should().BeTrue();
        reviewStatus5.IsSuccess.Should().BeTrue();
        reviewStatus6.IsSuccess.Should().BeFalse();
        reviewStatus7.IsSuccess.Should().BeFalse();

        reviewStatus2.Value.Value.Should().Be(IssueReviewStatus.WaitingForReviewer);
        reviewStatus3.Value.Value.Should().Be(IssueReviewStatus.OnReview);
        reviewStatus4.Value.Value.Should().Be(IssueReviewStatus.Accepted);
        reviewStatus5.Value.Value.Should().Be(IssueReviewStatus.AskedForRevision);
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
    public void Issue_CreateComment_WhenUserCreates()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueReviewStatus = IssueReviewStatusInfo.Create(1).Value;
        var reviewerId = ReviewerId.NewReviewerId();

        var message = Message.Create("test").Value;
        
        var comments = new List<Comment>()
        {
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(reviewerId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value
        };

        var newMessage = Message.Create("something").Value;

        var newComment = Comment.Create(
            CommentatorId.Create(userId.Value),
            newMessage, DateTime.UtcNow);
        
        var newComments = new List<Comment>()
        {
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(reviewerId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            newComment.Value
        };

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;
        
        var issueTakenTime = DateTime.UtcNow;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            comments,
            default,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            newComments,
            issueTakenTime,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        // act
        var createCommentRes = oldIssueReview.CreateComment(newComment.Value);

        // assert
        createCommentRes.IsSuccess.Should().BeTrue();
        oldIssueReview.Comments[0].Message.Should().Be(newIssueReview.Comments[0].Message);
        oldIssueReview.Comments[1].Message.Should().Be(newIssueReview.Comments[1].Message);
        oldIssueReview.Comments[2].Message.Should().Be(newIssueReview.Comments[2].Message);
        oldIssueReview.Comments[3].Message.Should().Be(newIssueReview.Comments[3].Message);
        newIssueReview.Comments[3].CommentatorId.Value.Should().Be(userId.Value);
    }
    
    [Fact]
    public void Issue_CreateComment_WhenReviewerCreates()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueReviewStatus = IssueReviewStatusInfo.Create(1).Value;
        var reviewerId = ReviewerId.NewReviewerId();

        var message = Message.Create("test").Value;
        
        var comments = new List<Comment>()
        {
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(reviewerId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value
        };

        var newMessage = Message.Create("something").Value;

        var newComment = Comment.Create(
            CommentatorId.Create(reviewerId.Value),
            newMessage, DateTime.UtcNow);
        
        var newComments = new List<Comment>()
        {
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(reviewerId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            newComment.Value
        };

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;
        
        var issueTakenTime = DateTime.UtcNow;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            reviewerId,
            comments,
            default,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        var newIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            reviewerId,
            newComments,
            issueTakenTime,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        // act
        var createCommentRes = oldIssueReview.CreateComment(newComment.Value);

        // assert
        createCommentRes.IsSuccess.Should().BeTrue();
        oldIssueReview.Comments[0].Message.Should().Be(newIssueReview.Comments[0].Message);
        oldIssueReview.Comments[1].Message.Should().Be(newIssueReview.Comments[1].Message);
        oldIssueReview.Comments[2].Message.Should().Be(newIssueReview.Comments[2].Message);
        oldIssueReview.Comments[3].Message.Should().Be(newIssueReview.Comments[3].Message);
        newIssueReview.Comments[3].CommentatorId.Value.Should().Be(reviewerId.Value);
    }
    
    [Fact]
    public void Issue_CreateComment_WhenCommentatorIsInvalid()
    {
        // arrange
        var issueId = IssueId.NewIssueId();
        var userId = UserId.NewUserId();
        var issueReviewStatus = IssueReviewStatusInfo.Create(1).Value;
        var reviewerId = ReviewerId.NewReviewerId();

        var message = Message.Create("test").Value;

        var comment = Comment.Create(CommentatorId.Create(Guid.NewGuid()), message, DateTime.UtcNow).Value;
        
        var comments = new List<Comment>()
        {
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(reviewerId.Value), message, DateTime.UtcNow).Value,
            Comment.Create(CommentatorId.Create(userId.Value), message, DateTime.UtcNow).Value
        };

        var pullRequestLink = PullRequestLink
            .Create(@"https://github.com/KirillSachkov/sachkov-tech/pull/4").Value;
        
        var issueTakenTime = DateTime.UtcNow;

        var oldIssueReview = IssueReview.IssueReview.Create(
            issueId,
            userId,
            issueReviewStatus,
            null,
            comments,
            default,
            DateTime.UtcNow,
            pullRequestLink).Value;
        
        // act
        var createCommentRes = oldIssueReview.CreateComment(comment);

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