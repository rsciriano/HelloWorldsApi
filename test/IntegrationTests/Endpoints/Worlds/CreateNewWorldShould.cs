using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Primitives;
using IntegrationTests.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTests.Endpoints.Worlds;

public abstract class CreateNewWorldShould<T> where T : class
{
    [Fact]
    public async Task Return_HTTP201_and_the_new_world_content_and_location_when_the_world_not_exist()
    {
        // Arrange
        var worldToBeCreated = new WorldModel { Id = 1, Name = "MyWorld" };

        // Act
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/worlds", worldToBeCreated);
        var responseContent = response.Content;

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.Created, "Response content: {0}", response.Content.ReadAsStringAsync().Result);
        (await responseContent.ReadFromJsonAsync<WorldModel>()).Should().BeEquivalentTo(worldToBeCreated);
        response.Headers.Location.Should().Be(new Uri(client.BaseAddress, $"api/worlds/{worldToBeCreated.Id}"));
    }

    [Theory]
    [InlineData(null, "")]
    public async Task Return_HTTP400_with_problem_validation_details_when_imput_model_is_invalid(int id, string? name)
    {
        // Arrange
        var worldToBeCreated = new WorldModel { Id = id, Name = name };
        var expecterResult = new ValidationProblemDetails(
            new Dictionary<string, string[]>()
            {
                { "Id", new [] { "The field Id must be between 1 and 2147483647." } },
                { "Name", new [] { "The Name field is required."} }
            }
        );

        // Act
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/worlds", worldToBeCreated);

        // Assert
        response.Should().HaveStatusCode(
            HttpStatusCode.BadRequest,
            "Response content: {0}",
            response.Content.ReadAsStringAsync().Result);

        response.Should().HaveJsonContentEquivalentTo(
            expecterResult,
            cfg => cfg.Including(x => x.Errors),
            "Response content: {0}",
            response.Content.ReadAsStringAsync().Result);
    }

    [Fact]
    public async Task Return_HTTP500_when_the_world_already_exist()
    {
        // Arrange
        var worldToBeCreated = new WorldModel { Id = 1, Name = "MyWorld" };

        // Act
        var client = CreateClient();

        var response1 = await client.PostAsJsonAsync("/api/worlds", worldToBeCreated);
        var responseContent1 = await response1.Content.ReadFromJsonAsync<WorldModel>();

        var response2 = await client.PostAsJsonAsync("/api/worlds", worldToBeCreated);

        // Assert
        response1.StatusCode.Should().Be(HttpStatusCode.Created);
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
