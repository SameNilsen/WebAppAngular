
//using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OsloMetAngular.Models
{
    // Data model (data schema) for DB
    // User, Comments and UserVotes are navigation properties.
    public class Post
    {
        [JsonPropertyName("PostId")]
        public int PostID { get; set; }

        //[RegularExpression(@"[0-9a-zA-Zæøå. \-]{2,80}", ErrorMessage = "The title must contain numbers " +
        //    "or letters, and must be between 2 and 80 characters.")]
        //[Display(Name = "Title")]
        [JsonPropertyName("Title")]
        public string Title { get; set; } = string.Empty;

        //[StringLength(200)]
        //[Display(Name = "Description")]
        //[Required]
        [JsonPropertyName("Text")]
        public string? Text { get; set; }
        [JsonPropertyName("ImageUrl")]
        public string? ImageUrl { get; set; }
        [JsonPropertyName("PostDate")]
        public string? PostDate { get; set; }
        [JsonPropertyName("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; } = default!;
        public virtual List<Comment>? Comments { get; set; }
        [JsonPropertyName("UpvoteCount")]
        public int UpvoteCount { get; set; } = 0;
        //public virtual List<Upvote>? UserVotes { get; set; }
        //[Display(Name = "Subforum")]
        [JsonPropertyName("SubForum")]
        public string SubForum { get; set; } = string.Empty;
    }
}
