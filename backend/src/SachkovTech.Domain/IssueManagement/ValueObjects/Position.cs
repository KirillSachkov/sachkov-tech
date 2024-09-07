using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.IssueManagement.ValueObjects;

public record Position
{
    public static readonly Position First = new(1);
    
    private Position(int value)
    {
        Value = value;
    }
    
    public int Value { get; }
    
    public static Result<Position, Error> Create(int number)
    {
        if (number <= 0)
            return Errors.General.ValueIsInvalid("serial number");

        return new Position(number);
    }
}