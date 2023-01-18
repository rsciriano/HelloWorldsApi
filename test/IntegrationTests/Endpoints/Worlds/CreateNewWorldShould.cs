using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IntegrationTests.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTests.Endpoints.Worlds;

public abstract class CreateNewWorldShould<T> where T : class
{
    [Fact]
    public async Task Return_HTTP200_and_the_new_world_when_the_world_not_exist()
    {
        // Arrange
        var worldToBeCreated = new WorldModel { Id = 1, Name = "World 1" };

        // Act
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/worlds", worldToBeCreated);
        var responseContent = await response.Content.ReadFromJsonAsync<WorldModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().BeEquivalentTo(worldToBeCreated);
    }

    [Fact]
    public async Task Return_HTTP500_when_the_world_already_exist()
    {
        // Arrange
        var worldToBeCreated = new WorldModel { Id = 1, Name = "World 1" };

        // Act
        var client = CreateClient();

        var response1 = await client.PostAsJsonAsync("/api/worlds", worldToBeCreated);
        var responseContent1 = await response1.Content.ReadFromJsonAsync<WorldModel>();

        var response2 = await client.PostAsJsonAsync("/api/worlds", worldToBeCreated);

        // Assert
        response1.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent1.Should().BeEquivalentTo(worldToBeCreated);
        response2.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    HttpClient CreateClient()
    {
        return _app.CreateClient();
    }

    public CreateNewWorldShould()
    {
        _app = new ApiApplication<T>();
        //_app = new ApiApplication("Production");
    }

    ApiApplication<T> _app;
}
