using System.Net;
using System.Net.Http.Json;
using MakApi.Models.Domain;
using MakTest.Fixtures;

namespace MakTest;

public class ProjectsControllerTests(WebApplicationFactoryFixture factory) : AbstractIntegrationTest(factory)
{
    [Fact]
    public async void TestDatabase_GetAll()
    {
        // Arrange 

        // Act
        var response = await Client.GetAsync(HttpHelper.Urls.GetAllProjects);
        var result = await response.Content.ReadFromJsonAsync<List<Project>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Count.Should().Be(Factory.InitialProjectCount);
        result.Should().BeEquivalentTo(DataFixture.GetProjects(Factory.InitialProjectCount),
            options => options.Excluding(t => t.Id));
    }

    [Fact]
    public async void TestDatabase_Create()
    {
        // Arrange
        var newProject = DataFixture.GetProject();

        // Act
        var request =
            await Client.PostAsync(HttpHelper.Urls.CreateProject, HttpHelper.GetStringContent(newProject));
        var response = await Client.GetAsync(HttpHelper.Urls.GetAllProjects);
        var result = await response.Content.ReadFromJsonAsync<List<Project>>();

        // Assert
        request.StatusCode.Should().Be(HttpStatusCode.Created);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Count.Should().Be(Factory.InitialProjectCount + 1);
    }
}