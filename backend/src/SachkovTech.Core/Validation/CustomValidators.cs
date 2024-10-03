using CSharpFunctionalExtensions;
using FluentValidation;

namespace SachkovTech.Core.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Error> result = factoryMethod(value);

            if (result.IsSuccess)
                return;

            context.AddFailure(result.Error.Serialize());
        });
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        return rule.WithMessage(error.Serialize());
    }
    
    public static IRuleBuilderOptionsConditions<T, string> MustBeProperExtension<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Custom((fn, context) =>
        {
            var result = Constants.Files.FORBIDDEN_FILE_EXTENSIONS
                .FirstOrDefault(ext => ext == Path.GetExtension(fn)) is null;

            if (result)
                return;
            
            context.AddFailure(Errors.Files.InvalidExtension().Serialize());
        });
    }
    
    public static IRuleBuilderOptionsConditions<T, Stream> MustBeProperSize<T>(
        this IRuleBuilder<T, Stream> ruleBuilder)
    {
        return ruleBuilder.Custom((fn, context) =>
        {
            var result = fn.Length < Constants.Files.MAX_FILE_SIZE;

            if (result)
                return;
            
            context.AddFailure(Errors.Files.InvalidSize().Serialize());
        });
    }
}