using MakTest.Fixtures;

namespace MakTest;

public class AbstractIntegrationTest : IClassFixture<WebApplicationFactoryFixture>
{
    protected readonly HttpClient Client;
    protected readonly WebApplicationFactoryFixture Factory;

    protected AbstractIntegrationTest(WebApplicationFactoryFixture factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
    }
}