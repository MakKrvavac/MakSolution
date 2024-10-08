namespace MakApi.Models.Domain;

//TODO: Delete this class
public class User
{
    public Guid Id { get; set; }

    public string SurName { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public byte password { get; set; }

    public List<Guid> FeedbackIds { get; set; }
    public List<Feedback> Feedbacks { get; set; }
}