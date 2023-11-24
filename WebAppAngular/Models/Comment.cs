using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OsloMetAngular.Models
{
    // Data model (data schema) for DB
    // User and Post are navigation properties.
    
    public class Comment
    {
        [JsonProperty("CommentID")]
        public int CommentID { get; set; }
        [JsonProperty("CommentText")]
        [StringLength(200)]
        public string CommentText { get; set; } = string.Empty;
        [JsonProperty("PostDate")]
        public string? PostDate { get; set; }
        [JsonProperty("UserId")]
        public int UserId { get; set; } = 1;
        //[Newtonsoft.Json.JsonIgnore]
        public virtual User User { get; set; } = default!;
        [JsonProperty("PostID")]
        public int PostID { get; set; }
        //[Newtonsoft.Json.JsonIgnore]
        public virtual Post Post { get; set; } = default!;


    }
}
