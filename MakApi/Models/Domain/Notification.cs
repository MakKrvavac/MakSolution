namespace MakApi.Models.Domain;

public class Notification
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public Guid UserId { get; set; }
    
    public User User { get; set; }
}