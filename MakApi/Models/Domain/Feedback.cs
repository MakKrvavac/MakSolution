namespace MakApi.Models.Domain;

public class Feedback
{
    public Guid Id { get; set; }

    public string FeedbackText { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; }

    public Guid TaskId { get; set; }
    public Task Task { get; set; }
}