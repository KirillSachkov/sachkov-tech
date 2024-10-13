using CSharpFunctionalExtensions;

namespace SachkovTech.SharedKernel.ValueObjects.Ids;

public class LessonId : ValueObject
{
    private LessonId(Guid? value)
    {
        Value = value;
    }

    public Guid? Value { get; }

    public static LessonId NewLessonId() => Guid.NewGuid();

    public static LessonId Empty() => new LessonId(null);

    public static LessonId Create(Guid? id) => new(id);

    public static implicit operator LessonId(Guid? id) => new(id);

    public static implicit operator Guid(LessonId lessonId)
    {
        ArgumentNullException.ThrowIfNull(lessonId);
        ArgumentNullException.ThrowIfNull(lessonId.Value);
        return lessonId.Value.Value;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        if (Value != null)
            yield return Value;
    }
}