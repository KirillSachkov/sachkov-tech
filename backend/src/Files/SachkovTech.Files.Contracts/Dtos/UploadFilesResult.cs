using SachkovTech.Files.Domain.ValueObjects;

namespace SachkovTech.Files.Contracts.Dtos;

public record UploadFilesResult(string BucketName, string FileName, FilePath FilePath, FileSize FileSize);