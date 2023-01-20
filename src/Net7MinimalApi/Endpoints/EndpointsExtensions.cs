namespace Net7MinimalApi.Endpoints;

public static class EndpointsExtensions
{
    public static RouteGroupBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/")
            .WithValidation();

        group.MapHelloEndpoints();
        group.MapWorldEndpoints();

        return group;
    }
}
