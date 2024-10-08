using MakApi.Models.Domain;
using File = MakApi.Models.Domain.File;
using Task = MakApi.Models.Domain.Task;

namespace MakApi.Models.Dtos;

public class ProjectDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? Start { get; set; }

    public DateOnly? End { get; set; }

    public string? PrototypeLink { get; set; }

    public string? DesignLink { get; set; }

    public string? GithubLink { get; set; }

    public string? LiveDemoLink { get; set; }

    public ProjectProgress? Progress { get; set; }

    public List<Guid>? FileIds { get; set; }
    public List<File>? Files { get; set; }

    public List<Guid>? TaskIds { get; set; }
    public List<Task>? Tasks { get; set; }
}