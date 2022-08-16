namespace Reactivities.Core.DTOs;

public class CommentDto
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string Body { get; set; }

    public string Username { get; set; }

    public string DisplayName { get; set; }

    public string Image { get; set; }
}