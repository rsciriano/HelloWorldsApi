using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IntegrationTests.Models;
using IntegrationTests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Endpoints.Hello;

public abstract class GetHelloShould<T> where T : class
{
    [Fact]
    public async Task Return_HTTP200_and_a_universal_greeting_when_no_name_is_provided()
    {
        // Act
        var client = CreateClient();
        var response = await client.GetAsync($"/api/hello");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK, $"ResponseContent: {responseContent}");
        responseContent.Should().Be("Hello world!");
    }

    [Theory]
    [InlineData("Rob")]
    [InlineData("Jonh")]
    public async Task Return_HTTP200_and_a_personal_greeting_when_a_name_is_provided(string name)
    {
        // Act
        var client = CreateClient();
        var response = await client.GetAsync($"/api/hello?name={name}");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK, $"ResponseContent: {responseContent}");
        responseContent.Should().Be($"Hello {name}!");
    }

    TSvr GetRequiredService<TSvr>() where TSvr : notnull
    {
        return _app.Services.GetRequiredService<TSvr>();
    }

    HttpClient CreateClient()
    {
        return _app.CreateClient();
    }

    public GetHelloShould()
    {
        _app = new ApiApplication<T>();
        //_app = new ApiApplication("Production");
    }

    ApiApplication<T> _app;
}
