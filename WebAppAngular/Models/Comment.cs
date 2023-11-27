using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OsloMetAngular.Models;

/* DATA MODEL (SCHEMA) FOR COMMENTS */
// User and Post are navigation properties.
public class Comment
{
    // CommentID
    [JsonProperty(nameof(CommentID))]
    public int CommentID { get; set; }

    // Description text
    [JsonProperty(nameof(CommentText))]
    [StringLength(200)]
    public string CommentText { get; set; } = string.Empty;

    // Post date
    [JsonProperty(nameof(PostDate))]
    public string? PostDate { get; set; }

    // UserID
    [JsonProperty(nameof(UserId))]
    public int UserId { get; set; } = 1;
    public virtual User User { get; set; } = default!;

    // PostID
    [JsonProperty(nameof(PostID))]
    public int PostID { get; set; }
    public virtual Post Post { get; set; } = default!;
}
