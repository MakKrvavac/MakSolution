namespace MakApi.Models.Domain;

public class Calendar
{
    public Guid Id { get; set; }

    public List<Guid>? EntryIds { get; set; }
    public CalendarEntry[]? Entries { get; set; }
}