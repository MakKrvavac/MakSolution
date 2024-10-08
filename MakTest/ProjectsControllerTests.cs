
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using MakApi.Models.Dtos;

namespace MakTest

{
    public class ProjectsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProjectsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/Projects");

            // Assert
            response.EnsureSuccessStatusCode();
            var projects = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();
            Assert.NotNull(projects);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_ForInvalidId()
        {
            // Act
            var response = await _client.GetAsync("/Projects/00000000-0000-0000-0000-000000000000");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ReturnsCreatedResponse()
        {
            // Arrange
            var newProject = new CreateProjectDto
            {
                Title = "New Project",
                Description = "Description of the new project",
                Start = new DateOnly(2023, 5, 17),
                End = new DateOnly(2023, 6, 17)
            };

            // Act
            var response = await _client.PostAsJsonAsync("/Projects", newProject);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdProject = await response.Content.ReadFromJsonAsync<ProjectDto>();
            Assert.NotNull(createdProject);
            Assert.Equal(newProject.Title, createdProject.Title);
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            var updateProject = new UpdateProjectDto
            {
                Title = "Updated Project",
                Description = "Updated description",
                Start = new DateOnly(2023, 5, 17),
                End = new DateOnly(2023, 6, 17)
            };

            // Act
            var response = await _client.PutAsJsonAsync("/Projects/00000000-0000-0000-0000-000000000001", updateProject);

            // Assert
            response.EnsureSuccessStatusCode();
            var updatedProject = await response.Content.ReadFromJsonAsync<ProjectDto>();
            Assert.NotNull(updatedProject);
            Assert.Equal(updateProject.Title, updatedProject.Title);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResponse()
        {
            // Act
            var response = await _client.DeleteAsync("/Projects/00000000-0000-0000-0000-000000000001");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}