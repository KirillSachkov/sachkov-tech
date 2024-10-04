using CSharpFunctionalExtensions;

namespace SachkovTech.SharedKernel.ValueObjects.Ids;

public class ModuleId : ValueObject
{
    private ModuleId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ModuleId NewModuleId() => new(Guid.NewGuid());

    public static ModuleId Empty() => new(Guid.Empty);

    public static ModuleId Create(Guid id) => new(id);

    public static implicit operator ModuleId(Guid id) => new(id);

    public static implicit operator Guid(ModuleId moduleId)
    {
        ArgumentNullException.ThrowIfNull(moduleId);
        return moduleId.Value;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}