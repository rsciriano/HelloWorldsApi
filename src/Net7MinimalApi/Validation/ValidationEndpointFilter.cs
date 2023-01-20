using System.Reflection;
using FluentValidation;
using Net7MinimalApi.Extensions;

namespace Net7MinimalApi.Validation;

public class ValidationEndpointFilter : IEndpointFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationEndpointFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var genericValidatorType = typeof(IValidator<>);
        foreach (var argument in context.Arguments)
        {
            if (argument is null) continue;
            var argumentType = argument.GetType();
            if (!argumentType.IsClass) continue;

            var validatorType = genericValidatorType.MakeGenericType(argumentType);
            var validator = (IValidator)_serviceProvider.GetService(validatorType);
            if (validator is null) continue;

            var validationTotextType = typeof(ValidationContext<>).MakeGenericType(argumentType);
            var validationContext = (IValidationContext)Activator.CreateInstance(validationTotextType, argument);
            var validationResult = await validator.ValidateAsync(validationContext);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.AsValidationProblemDetails());
            }

            break;
        }

        return await next(context);
    }
}
