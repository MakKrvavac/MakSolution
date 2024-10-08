namespace MakApi.Models.Domain;

public class Task
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public Guid ResponsibleUserId { get; set; }
    public User ResponsibleUser { get; set; }

    public double StoryPoints { get; set; }

    public Guid EpicId { get; set; }
    public Epic Epic { get; set; }

    public TaskProgress TaskProgress { get; set; }
}