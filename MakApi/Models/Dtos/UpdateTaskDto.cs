using MakApi.Models.Domain;

namespace MakApi.Models.Dtos;

public class UpdateTaskDto
{
    public string Title { get; set; }

    public string Description { get; set; }

    public Guid ResponsibleUserId { get; set; }
    public User ResponsibleUser { get; set; }

    public double StoryPoints { get; set; }

    public Guid EpicId { get; set; }
    public Epic Epic { get; set; }
}