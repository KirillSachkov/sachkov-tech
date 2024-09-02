using SachkovTech.Domain.IssueManagement.ValueObjects;

namespace SachkovTech.Application.FileProvider;

public record FileData(Stream Stream, FilePath FilePath, string BucketName);