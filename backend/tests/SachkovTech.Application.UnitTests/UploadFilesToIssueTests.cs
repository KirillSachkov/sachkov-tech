using System.Reflection;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using SachkovTech.Application.Database;
using SachkovTech.Application.Dtos;
using SachkovTech.Application.FileProvider;
using SachkovTech.Application.Modules;
using SachkovTech.Application.Modules.UploadFilesToIssue;
using SachkovTech.Domain.IssueManagement.Entities;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;
using Module = SachkovTech.Domain.IssueManagement.Module;

namespace SachkovTech.Application.UnitTests;

public class UploadFilesToIssueTests
{
    [Fact]
    public async Task Handle_Should_Upload_Files_To_Issue()
    {
        // arrange
        var ct = new CancellationTokenSource().Token;

        var title = Title.Create("Test").Value;
        var description = Description.Create("Description").Value;
        var module = new Module(ModuleId.NewModuleId(), title, description);
        var issue = new Issue(IssueId.NewIssueId(), title, description, LessonId.Empty(), null, null);

        module.AddIssue(issue);

        var stream = new MemoryStream();
        var fileName = "test.jpg";

        var uploadFileDto = new UploadFileDto(stream, fileName);

        List<UploadFileDto> files = [uploadFileDto, uploadFileDto];

        var command = new UploadFilesToIssueCommand(module.Id.Value, issue.Id.Value, files);

        var fileProviderMock = new Mock<IFileProvider>();

        List<FilePath> filePaths =
        [
            FilePath.Create(fileName).Value,
            FilePath.Create(fileName).Value,
        ];

        fileProviderMock
            .Setup(v => v.UploadFiles(It.IsAny<List<FileData>>(), ct))
            .ReturnsAsync(Result.Success<IReadOnlyList<FilePath>, Error>(filePaths));

        var modulesRepositoryMock = new Mock<IModulesRepository>();

        modulesRepositoryMock.Setup(m => m.GetById(module.Id, ct))
            .ReturnsAsync(Result.Success<Module, Error>(module));

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);

        var validatorMock = new Mock<IValidator<UploadFilesToIssueCommand>>();

        validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());
        
        var loggerMock = new Mock<ILogger<UploadFilesToIssueHandler>>();

        var handler = new UploadFilesToIssueHandler(
            fileProviderMock.Object,
            modulesRepositoryMock.Object,
            unitOfWorkMock.Object,
            validatorMock.Object,
            loggerMock.Object);

        // act
        var result = await handler.Handle(command, ct);

        // assert
        module.Issues.First(i => i.Id == issue.Id).Files.Should().HaveCount(2);
        result.IsSuccess.Should().BeTrue();
    }

    // public class ModulesRepositoryTest : IModulesRepository
    // {
    //     public Task<Guid> Add(Module module, CancellationToken cancellationToken = default)
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     public Guid Save(Module module, CancellationToken cancellationToken = default)
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     public Guid Delete(Module module, CancellationToken cancellationToken = default)
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     public async Task<Result<Module, Error>> GetById(ModuleId moduleId, CancellationToken cancellationToken = default)
    //     {
    //         var title = Title.Create("Test").Value;
    //         var description = Description.Create("Description").Value;
    //         var module = new Module(ModuleId.NewModuleId(), title, description);
    //         
    //         return await Task.FromResult(module);
    //     }
    //
    //     public Task<Result<Module, Error>> GetByTitle(Title title, CancellationToken cancellationToken = default)
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}