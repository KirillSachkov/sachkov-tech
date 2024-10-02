using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.IssueSolvingManagement.ValueObjects;

public class Attempts : ValueObject
{
    private Attempts(int value)
    {
        Value = value;
    }
    
    public int Value { get; }

    public Attempts Add() => Create(Value + 1);

    public static Attempts Create(int attempts = 1)
    {
        return new Attempts(attempts);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}