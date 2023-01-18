using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IntegrationTests.Models;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Endpoints.Worlds;

public abstract class GetWorldByIdShould<T> where T : class
{
    [Fact]
    public async Task Return_HTTP200_and_the_world_when_the_world_exist()
    {
        // Arrange
        var testDataManager = GetRequiredService<TestDataManager>();
        var existingWorld = await testDataManager.CreateAndSaveNewWorld();


        // Act
        var client = CreateClient();
        var response = await client.GetAsync($"/api/worlds/{existingWorld.Id}");
        var responseContent = await response.Content.ReadFromJsonAsync<WorldModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().BeEquivalentTo(existingWorld);
    }

    TSvr GetRequiredService<TSvr>() where TSvr : notnull
    {
        return _app.Services.GetRequiredService<TSvr>();
    }

    HttpClient CreateClient()
    {
        return _app.CreateClient();
    }

    public GetWorldByIdShould()
    {
        _app = new ApiApplication<T>();
        //_app = new ApiApplication("Production");
    }

    ApiApplication<T> _app;
}
