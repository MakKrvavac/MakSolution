using System.Net;
using System.Net.Http.Json;
using System.Text;
using MakApi;
using MakApi.Data;
using MakApi.Models.Domain;
using MakApi.Models.Dtos;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

namespace MakTest.Common;

public class InMemoryDatabase
{
    private static readonly string _username = "testuser";
    private static readonly string _password = "test@123";

    private readonly WebApplicationFactory<Program> _factory;

    public InMemoryDatabase()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<MakDbContext>));
                services.AddDbContext<MakDbContext>(options => { options.UseInMemoryDatabase("TestDb"); });
            });
        });
    }

    [Fact]
    public async void TestInMemory_GetAll()
    {
        // Arrange 
        using (var scope = _factory.Services.CreateScope())
        {
            var scoopedServices = scope.ServiceProvider;
            var dbContext = scoopedServices.GetRequiredService<MakDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.Projects.Add(new Project
            {
                Id = Guid.NewGuid(),
                Title = "Project 1"
            });
            dbContext.SaveChanges();
        }

        var client = _factory.CreateClient();
        //await RegisterUser(client);
        //var token = await GetAuthTokenAsync(client);
        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await client.GetAsync(HttpHelper.Urls.GetAllProjects);
        var result = await response.Content.ReadFromJsonAsync<List<Project>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Count.Should().Be(1);
        result[0].Title.Should().Be("Project 1");
    }


    [Fact]
    public async void TestInMemory_Create()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scoopedServices = scope.ServiceProvider;
            var dbContext = scoopedServices.GetRequiredService<MakDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
        }

        var client = _factory.CreateClient();
        //await RegisterUser(client);
        //var token = await GetAuthTokenAsync(client);
        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var newProject = new CreateProjectDto
        {
            Title = "Project 1"
        };

        //var httpContent = new StringContent(JsonConvert.SerializeObject(newTask), Encoding.UTF8, "application/json");

        // Act
        var jsonContent = new StringContent(
            JsonConvert.SerializeObject(newProject),
            Encoding.UTF8,
            "application/json"
        );


        var request = await client.PostAsync(HttpHelper.Urls.CreateProject, jsonContent);
        if (!request.IsSuccessStatusCode)
        {
            var errorResponse = await request.Content.ReadAsStringAsync();
            throw new Exception($"Error: {request.StatusCode}, Details: {errorResponse}");
        }

        var response = await client.GetAsync(HttpHelper.Urls.GetAllProjects);
        var result = await response.Content.ReadFromJsonAsync<List<Project>>();

        // Assert
        request.StatusCode.Should().Be(HttpStatusCode.Created);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Count.Should().Be(1);
        result[0].Title.Should().Be("Project 1");
    }


    private async Task<string> RegisterUser(HttpClient client)
    {
        // Replace with your actual authentication endpoint and credentials
        var response =
            await client.PostAsJsonAsync(HttpHelper.Urls.Register, new { Username = _username, Password = _password });
        Console.WriteLine("TEST");
        Console.WriteLine(response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent); // Log the response content

        var authResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        return authResponse.JwtToken;
    }


    private async Task<string> GetAuthTokenAsync(HttpClient client)
    {
        var response =
            await client.PostAsJsonAsync(HttpHelper.Urls.Login,
                new RegisterRequestDto { Username = _username, Password = _password, Roles = ["writer"] });
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent); // Log the response content

        var authResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        return authResponse.JwtToken;
    }
}