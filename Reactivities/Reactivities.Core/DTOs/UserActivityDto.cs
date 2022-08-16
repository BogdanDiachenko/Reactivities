using System.Text.Json.Serialization;

namespace Reactivities.Core.DTOs;

public class UserActivityDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Category { get; set; }

    public DateTimeOffset Date { get; set; }

    [JsonIgnore]
    public string  HostUsername { get; set; }
}