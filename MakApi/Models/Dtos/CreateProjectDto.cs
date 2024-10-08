namespace MakApi.Models.Dtos;

public class CreateProjectDto
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? Start { get; set; }

    public DateOnly? End { get; set; }
}