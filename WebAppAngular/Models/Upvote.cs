using Newtonsoft.Json;

namespace OsloMetAngular.Models;

/* DATA MODEL (SCHEMA) FOR VOTES */
// User and Post are navigation properties.
public class Upvote
{
    // VoteID
    [JsonProperty(nameof(UpvoteId))]
    public int UpvoteId { get; set; }

    // The actual vote data (up or down)
    [JsonProperty(nameof(Vote))]
    public string Vote { get; set; } = string.Empty;

    // UserID
    [JsonProperty(nameof(UserId))]
    public int UserId { get; set; }

    // Virtual user
    public virtual User User { get; set; } = default!;

    // PostID
    [JsonProperty(nameof(PostID))]
    public int PostID { get; set; }

    // Virtual post
    public virtual Post Post { get; set; } = default!;     
}
