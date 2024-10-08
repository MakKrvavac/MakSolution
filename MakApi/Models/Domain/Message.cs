namespace MakApi.Models.Domain;

public class Message
{
    public Guid Id { get; set; }

    public Guid SenderId { get; set; }
    public User Sender { get; set; }

    public Guid ReceiverId { get; set; }
    public User Receiver { get; set; }

    public string Text { get; set; }

    public DateTime Timestamp { get; set; }
    
}