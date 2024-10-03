using CSharpFunctionalExtensions;

namespace SachkovTech.Core.ValueObjects.Ids;

public class FileId : ValueObject
{
    private FileId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static FileId NewFileId() => new(Guid.NewGuid());
    public static FileId Empty() => new(Guid.Empty);
    public static FileId Create(Guid id) => new(id);

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}