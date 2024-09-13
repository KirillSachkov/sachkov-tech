using SachkovTech.Domain.IssueManagement.ValueObjects;

namespace SachkovTech.Application.Files;

public record FileData(Stream Stream, FileInfo Info);

public record FileInfo(FilePath FilePath, string BucketName);