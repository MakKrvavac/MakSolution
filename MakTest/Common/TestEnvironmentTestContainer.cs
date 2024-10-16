using System.Net;
using System.Net.Http.Json;
using MakApi.Models.Domain;
using MakTest.Fixtures;

namespace MakTest.Common;

public class TestEnvironmentTestContainer : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactoryFixture _factory;

    public TestEnvironmentTestContainer(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async void TestDatabase_GetAll()
    {
        // Arrange 

        // Act
        var response = await _client.GetAsync(HttpHelper.Urls.GetAllProjects);
        var result = await response.Content.ReadFromJsonAsync<List<Project>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Count.Should().Be(_factory.InitialProjectCount);
        result.Should().BeEquivalentTo(DataFixture.GetProjects(_factory.InitialProjectCount),
            options => options.Excluding(t => t.Id));
    }

    [Fact]
    public async void TestDatabase_Create()
    {
        // Arrange
        var newProject = DataFixture.GetProject();

        // Act
        var request =
            await _client.PostAsync(HttpHelper.Urls.CreateProject, HttpHelper.GetStringContent(newProject));
        var response = await _client.GetAsync(HttpHelper.Urls.GetAllProjects);
        var result = await response.Content.ReadFromJsonAsync<List<Project>>();

        // Assert
        request.StatusCode.Should().Be(HttpStatusCode.Created);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Count.Should().Be(_factory.InitialProjectCount + 1);
    }
}