using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OsloMetAngular.Models;

/* DATA MODEL FOR ApplicationUser */
public class ApplicationUser : IdentityUser
{
    // UserID
    [JsonPropertyName("UserId")]
    public int UserId { get; set; }

    // Name
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    // List of comments
    public virtual List<Comment>? Comments { get; set; }

    // IdentityUserID
    [JsonPropertyName("IdentityUserId")]
    [ForeignKey("IdentityUser")]
    public string? IdentityUserId { get; set; }

    // List of posts
    public virtual List<Post>? Posts { get; set; }

    // List of user votes
    public virtual List<Upvote>? UserVotes { get; set; }

    // Credebility
    [JsonPropertyName("Credebility")]
    public int Credebility { get; set; }
}
