namespace MakApi.Models.Domain;

public class Meeting : CalendarEntry
{
    public Guid Id { get; set; }

    public string Place { get; set; }

    public string Link { get; set; }
    
}