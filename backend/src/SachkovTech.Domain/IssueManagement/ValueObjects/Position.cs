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

    public Result<Position, Error> Forward() =>
        Create(Value + 1);

    public Result<Position, Error> Back() =>
        Create(Value - 1);

    public static Result<Position, Error> Create(int number)
    {
        if (number < 1)
            return Errors.General.ValueIsInvalid("serial number");

        return new Position(number);
    }
}