namespace MakApi.Models.Domain;

public class File
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public long Version { get; set; }

    public DateTime Timestamp { get; set; }

    public Guid BlobStorageId { get; set; }
    
}