using Bogus;
using MakApi.Models.Domain;

namespace MakTest.Fixtures;

public class DataFixture
{
    public static List<Project> GetProjects(int count, bool useNewSeed = false)
    {
        return GetProjectFaker(useNewSeed).Generate(count);
    }

    public static Project GetProject(bool useNewSeed = false)
    {
        return GetProjects(1, useNewSeed)[0];
    }

    private static Faker<Project> GetProjectFaker(bool useNewSeed)
    {
        var seed = 0;
        if (useNewSeed) seed = Random.Shared.Next(10, int.MaxValue);
        return new Faker<Project>()
            .RuleFor(t => t.Id, f => Guid.NewGuid())
            .RuleFor(t => t.Title, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
            .UseSeed(seed);
    }
}