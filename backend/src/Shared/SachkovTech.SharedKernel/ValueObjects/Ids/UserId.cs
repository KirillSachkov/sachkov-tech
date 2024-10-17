using CSharpFunctionalExtensions;

namespace SachkovTech.SharedKernel.ValueObjects.Ids;

public class UserId : ValueObject
{
    // ef core
    private UserId()
    {
        
    }
    private UserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static UserId NewUserId() => new(Guid.NewGuid());
    public static UserId Empty() => new(Guid.Empty);
    public static UserId Create(Guid id) => new(id);
    
    public static implicit operator UserId(Guid id) => new(id);

    public static implicit operator Guid(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        return userId.Value;
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}