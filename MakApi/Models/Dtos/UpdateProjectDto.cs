using MakApi.Models.Domain;

namespace MakApi.Models.Dtos;

public class UpdateProjectDto
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? Start { get; set; }

    public DateOnly? End { get; set; }

    public string? PrototypeLink { get; set; }

    public string? DesignLink { get; set; }

    public string? GithubLink { get; set; }

    public string? LiveDemoLink { get; set; }

    public ProjectProgress? Progress { get; set; }

    public List<Guid>? FileIds { get; set; }

    public List<Guid>? TaskIds { get; set; }
}