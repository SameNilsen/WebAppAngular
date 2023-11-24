using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OsloMetAngular.Models
{
    // Data model (data schema) for DB
    // Comments, IdentityUser, UserVotes and Posts are navigation properties.
    public class User
    {
        [JsonProperty("UserId")]
        public int UserId { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; } = string.Empty;
        public virtual List<Comment>? Comments { get; set; }
        [JsonProperty("IdentityUserId")]
        [ForeignKey("IdentityUser")]
        public string? IdentityUserId { get; set; }
        //public virtual IdentityUser? IdentityUser { get; set; }  //  Link to the IdentityUser.
        public virtual List<Post>? Posts { get; set; }
        public virtual List<Upvote>? UserVotes { get; set; }
        [JsonProperty("Credebility")]
        public int Credebility { get; set; }
    }
}
