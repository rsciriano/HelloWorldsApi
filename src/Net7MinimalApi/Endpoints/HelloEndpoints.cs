namespace Net7MinimalApi.Endpoints;

public static class HelloEndpoints
{
    public static RouteGroupBuilder MapHelloEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("api/hello", GetHello)
            .WithName("Hello")
            .WithTags("Hello");

        return group;
    }

    private static string GetHello(string? name)
    {
        return $"Hello {name ?? "world"}!";
    }
}
