using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Domain.ValueObjects;

public class Experience : ValueObject
{
    private const int MAX_VALUE = 1000;
    public int Value { get; }

    private Experience(int value)
    {
        Value = value;
    }

    public static Result<Experience, Error> Create(int experience)
    {
        if (experience is < 0 or > MAX_VALUE)
            return Errors.General.ValueIsInvalid("Experience");
        return new Experience(experience);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}