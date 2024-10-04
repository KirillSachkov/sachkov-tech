// using FluentAssertions;
// using SachkovTech.Core.ValueObjects.Ids;
// using SachkovTech.Domain.IssueSolvingManagement.Entities;
// using SachkovTech.Domain.IssueSolvingManagement.ValueObjects;
//
// namespace SachkovTech.Domain.UnitTests;
//
// public class IssueSolvingTests
// {
//     [Fact]
//     public void Complete_Issue_On_The_First_Try()
//     {
//         // arrange
//         var taskIdentifier = new IssueIdentifier(ModuleId.Empty(), Guid.Empty);
//
//         var userTask = new UserIssue(UserIssueId.Empty(), Guid.Empty, taskIdentifier);
//
//         var pullRequest = PullRequestUrl.Create("https://github.com/name/repo/pull/15").Value;
//         
//         userTask.TakeTask();
//         userTask.SendOnReview(pullRequest);
//         
//         // act
//         userTask.CompleteTask();
//         
//         // assert
//         userTask.Attempts.Value.Should().Be(1);
//         userTask.Status.Should().Be(IssueStatus.Completed);
//     }
//
//     [Fact]
//     public void Complete_Issue_On_The_Second_Try()
//     {
//         // arange
//         var taskIdentifier = new IssueIdentifier(ModuleId.Empty(), Guid.Empty);
//         
//         var userTask = new UserIssue(UserIssueId.Empty(), Guid.Empty, taskIdentifier);
//         
//         var pullRequest = PullRequestUrl.Create("https://github.com/name/repo/pull/15").Value;
//         
//         userTask.TakeTask();
//         userTask.SendOnReview(pullRequest);
//         userTask.SendForRevision();
//         userTask.SendOnReview(pullRequest);
//
//         // act
//         userTask.CompleteTask();
//         
//         // assert
//         userTask.Attempts.Value.Should().Be(2);
//         userTask.Status.Should().Be(IssueStatus.Completed);
//
//     }
// }