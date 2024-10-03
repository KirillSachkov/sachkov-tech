using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.FilesManagement.ValueObjects;

public class OwnerType : ValueObject
{
    public static readonly OwnerType Issue = new(nameof(Issue).ToUpper());

    private static readonly OwnerType[] _types = [Issue];

    public string Value { get; }

    private OwnerType(string value)
    {
        Value = value;
    }

    public static Result<OwnerType, Error> Create(string input)
    {
        var fileOwnerType = input.Trim().ToUpper();

        if (_types.Any(t => t.Value == fileOwnerType) == false)
        {
            return Errors.General.ValueIsInvalid("owner type");
        }

        return new OwnerType(fileOwnerType);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(OwnerType ownerType) =>
        ownerType.Value;
}