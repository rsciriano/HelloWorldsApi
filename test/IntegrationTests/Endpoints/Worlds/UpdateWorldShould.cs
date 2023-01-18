using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IntegrationTests.Models;
using IntegrationTests.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Endpoints.Worlds;

public abstract class UpdateWorldShould<T> where T : class
{
    [Fact]
    public async Task Return_HTTP200_and_the_world_when_the_world_exist()
    {
        // Arrange
        var testDataManager = GetRequiredService<TestDataManager>();
        var existingAndModifiedWorld = await testDataManager.CreateAndSaveNewWorld();
        existingAndModifiedWorld.Name = $"{existingAndModifiedWorld.Name} Changed";


        // Act
        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/worlds", existingAndModifiedWorld);
        var responseContent = await response.Content.ReadFromJsonAsync<WorldModel>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().BeEquivalentTo(existingAndModifiedWorld);
    }

    TSvr GetRequiredService<TSvr>() where TSvr : notnull
    {
        return _app.Services.GetRequiredService<TSvr>();
    }

    HttpClient CreateClient()
    {
        return _app.CreateClient();
    }

    public UpdateWorldShould()
    {
        _app = new ApiApplication<T>();
        //_app = new ApiApplication("Production");
    }

    ApiApplication<T> _app;
}
