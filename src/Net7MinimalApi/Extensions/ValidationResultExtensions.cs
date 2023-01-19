using FluentValidation.Results;

namespace Net7MinimalApi.Extensions;

public static class ValidationResultExtensions
{
    public static HttpValidationProblemDetails AsValidationProblemDetails(this IEnumerable<ValidationFailure> failures)
    {
        return new HttpValidationProblemDetails(failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray()));
    }
}
