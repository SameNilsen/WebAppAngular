using Newtonsoft.Json; // Because we are using Newtonssoft for serializing.

namespace OsloMetAngular.Models;

/* DATA MODEL (SCHEMA) FOR POSTS */
// User, Comments and UserVotes are navigation properties.
public class Post
{
    // PostID
    [JsonProperty("PostId")]
    public int PostID { get; set; }

    // Title
    [JsonProperty(nameof(Title))]
    public string Title { get; set; } = string.Empty;

    // Description text
    [JsonProperty(nameof(Text))]
    public string? Text { get; set; }

    // Image URL
    [JsonProperty(nameof(ImageUrl))]
    public string? ImageUrl { get; set; }

    // Post date
    [JsonProperty(nameof(PostDate))]
    public string? PostDate { get; set; }

    // UserID
    [JsonProperty(nameof(UserId))]
    public int UserId { get; set; }

    // Virtual user
    public virtual User User { get; set; } = default!;

    // List of comments
    public virtual List<Comment>? Comments { get; set; }

    // Vote count
    [JsonProperty(nameof(UpvoteCount))]
    public int UpvoteCount { get; set; } = 0;

    // List of user votes
    public virtual List<Upvote>? UserVotes { get; set; }

    // Subforum name
    [JsonProperty(nameof(SubForum))]
    public string SubForum { get; set; } = string.Empty;
}
