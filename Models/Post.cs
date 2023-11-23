
//using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;  //  Because we are using Newtonssoft for serializing.
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OsloMetAngular.Models
{
    // Data model (data schema) for DB
    // User, Comments and UserVotes are navigation properties.
    public class Post
    {
        [JsonProperty("PostId")]
        public int PostID { get; set; }

        //[RegularExpression(@"[0-9a-zA-Zæøå. \-]{2,80}", ErrorMessage = "The title must contain numbers " +
        //    "or letters, and must be between 2 and 80 characters.")]
        //[Display(Name = "Title")]
        [JsonProperty("Title")]
        public string Title { get; set; } = string.Empty;

        //[StringLength(200)]
        //[Display(Name = "Description")]
        //[Required]
        [JsonProperty("Text")]
        public string? Text { get; set; }
        [JsonProperty("ImageUrl")]
        public string? ImageUrl { get; set; }
        [JsonProperty("PostDate")]
        public string? PostDate { get; set; }
        [JsonProperty("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; } = default!;
        public virtual List<Comment>? Comments { get; set; }
        [JsonProperty("UpvoteCount")]
        public int UpvoteCount { get; set; } = 0;
        public virtual List<Upvote>? UserVotes { get; set; }
        //[Display(Name = "Subforum")]
        [JsonProperty("SubForum")]
        public string SubForum { get; set; } = string.Empty;
        //public int ApplicationUserId { get; set; }
        //public virtual ApplicationUser ApplicationUser { get; set; } = default!;
    }
}
