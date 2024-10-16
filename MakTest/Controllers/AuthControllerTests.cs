using System.Net.Http.Json;
using MakApi.Models.Dtos;
using MakTest.Fixtures;

namespace MakTest;

public class AuthControllerTests(WebApplicationFactoryFixture factory) : AbstractIntegrationTest(factory)
{
    private async Task<string> RegisterUser(HttpClient client)
    {
        // Replace with your actual authentication endpoint and credentials
        var response =
            await client.PostAsJsonAsync(HttpHelper.Urls.Register,
                new LoginRequestDto
                {
                    Username = HttpHelper.Credentials.Username,
                    Password = HttpHelper.Credentials.Password
                });
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
                new RegisterRequestDto
                {
                    Username = HttpHelper.Credentials.Username,
                    Password = HttpHelper.Credentials.Password,
                    Roles = ["writer"]
                });
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent); // Log the response content

        var authResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        return authResponse.JwtToken;
    }
}