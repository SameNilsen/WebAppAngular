
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OsloMetAngular.Models
{
    // Data model (data schema) for DB
    // User and Post are navigation properties.
    public class Upvote
    {
        [JsonProperty("UpvoteId")]
        public int UpvoteId { get; set; }
        [JsonProperty("Vote")]
        public string Vote { get; set; } = string.Empty;
        [JsonProperty("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; } = default!;
        [JsonProperty("PostID")]
        public int PostID { get; set; }
        public virtual Post Post { get; set; } = default!;     
    }
}
