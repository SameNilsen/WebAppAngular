
using System.Text.Json.Serialization;

namespace OsloMetAngular.Models
{
    // Data model (data schema) for DB
    // User and Post are navigation properties.
    public class Upvote
    {
        [JsonPropertyName("UpvoteId")]
        public int UpvoteId { get; set; }
        [JsonPropertyName("Vote")]
        public string Vote { get; set; } = string.Empty;
        [JsonPropertyName("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; } = default!;
        [JsonPropertyName("PostID")]
        public int PostID { get; set; }
        public virtual Post Post { get; set; } = default!;     
    }
}
