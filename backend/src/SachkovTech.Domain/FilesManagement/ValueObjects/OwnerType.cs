using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.FilesManagement.ValueObjects;

public class OwnerType : ValueObject
{
    public static readonly OwnerType Issue = new(nameof(Issue).ToUpper());

    public string Value { get; }

    private OwnerType(string value)
    {
        Value = value;
    }

    public static Result<OwnerType, Error> Create(string fileOwnerType)
    {
        if (string.IsNullOrWhiteSpace(fileOwnerType) || fileOwnerType.Length < Constants.Default.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("file owner type");

        return new OwnerType(fileOwnerType);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(OwnerType ownerType) =>
        ownerType.Value;
}