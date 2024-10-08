namespace MakApi.Models.Domain;

public class CalendarEntry
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTime DateTime { get; set; }
}