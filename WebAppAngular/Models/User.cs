using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace OsloMetAngular.Models;

/* DATA MODEL (SCHEMA) FOR USER */
// Comments, IdentityUser, UserVotes and Posts are navigation properties.
public class User
{
    // UserID
    [JsonProperty(nameof(UserId))]
    public int UserId { get; set; }

    // Name
    [JsonProperty(nameof(Name))]
    public string Name { get; set; } = string.Empty;

    // List of comments
    public virtual List<Comment>? Comments { get; set; }

    // IdentityUserID
    [JsonProperty(nameof(IdentityUserId))]
    [ForeignKey("IdentityUser")]
    public string? IdentityUserId { get; set; }

    // List of posts
    public virtual List<Post>? Posts { get; set; }

    // List of user votes
    public virtual List<Upvote>? UserVotes { get; set; }

    // Credebility
    [JsonProperty(nameof(Credebility))]
    public int Credebility { get; set; }
}
