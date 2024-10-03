using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using SachkovTech.Core;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.ValueObjects;
using SachkovTech.Core.ValueObjects.Ids;
using Module = SachkovTech.Domain.IssueManagement.Module;

namespace SachkovTech.Application.UnitTests;

public class UploadFilesToIssueTests
{
    private readonly Mock<IFileProvider> _fileProviderMock = new();
    private readonly Mock<IModulesRepository> _modulesRepositoryMock = new();
    private readonly Mock<ILogger<UploadFilesToIssueHandler>> _loggerMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IValidator<UploadFilesToIssueCommand>> _validatorMock = new();

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

        List<FilePath> filePaths =
        [
            FilePath.Create(fileName).Value,
            FilePath.Create(fileName).Value,
        ];

        _fileProviderMock
            .Setup(v => v.UploadFiles(It.IsAny<List<FileData>>(), ct))
            .ReturnsAsync(Result.Success<IReadOnlyList<FilePath>, Error>(filePaths));

        _modulesRepositoryMock.Setup(m => m.GetById(module.Id, ct))
            .ReturnsAsync(Result.Success<Module, Error>(module));

        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);

        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());
        
        var handler = new UploadFilesToIssueHandler(
            _fileProviderMock.Object,
            _modulesRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);

        // act
        var result = await handler.Handle(command, ct);

        // assert
        module.Issues.First(i => i.Id == issue.Id).Files.Should().HaveCount(2);
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task Handle_Should_Return_Error_When_Module_Does_Not_Exist()
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
}