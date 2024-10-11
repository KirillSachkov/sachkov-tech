using SachkovTech.Files.Domain.ValueObjects;

namespace SachkovTech.Files.Contracts.Dtos;

public record FileLocation(string BucketName, FilePath FilePath);