using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OsloMetAngular.Models
{
    // Data model (data schema) for DB
    // User and Post are navigation properties.
    
    public class Comment
    {
        [JsonPropertyName("CommentID")]
        public int CommentID { get; set; }
        [JsonPropertyName("CommentText")]
        [StringLength(200)]
        public string CommentText { get; set; } = string.Empty;
        [JsonPropertyName("PostDate")]
        public string? PostDate { get; set; }
        [JsonPropertyName("UserId")]
        public int UserId { get; set; } = 1;
        public virtual User User { get; set; } = default!;
        [JsonPropertyName("PostID")]
        public int PostID { get; set; }
        public virtual Post Post { get; set; } = default!;


    }
}
