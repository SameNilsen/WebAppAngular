using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using OsloMetAngular.DAL;
using OsloMetAngular.Models;
using OsloMetAngular.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OsloMetAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Console.Write("usercontroller1");
            var users = await _userRepository.GetAll();
            if (users == null)
            {
                _logger.LogError("[UserController] User list not found when executing _userRepository.GetAll(),");
                return NotFound("User list not found");
            }
            return Ok(users);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] User newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Invalid user data");
            }
            bool returnOk = await _userRepository.Create(newUser);

            if (returnOk)
            {
                var response = new { success = true, message = "User " + newUser.Name + " created succesfully" };
                return Ok(response);                
            }
            else
            {
                var response = new { success = false, message = "User creation failed" };
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserbyId(int id)
        {
            var user = await _userRepository.GetItemById(id);
            if (user == null)
            {
                _logger.LogError("[UserController] User list not found when executing _userRepository.GetAll(),");
                return NotFound("User list not found");
            }
            return Ok(user);
        }

        [HttpGet("simpleuser/{id}")]
        public async Task<IActionResult> GetSimplifiedUser(int id)
        {
            var user = await _userRepository.GetItemById(id);
            if (user == null)
            {
                _logger.LogError("[UserController] User not found while executing" +
                    "_userRepository.GetItemById(id)", id);
                return NotFound("Did not find user");
            }
            
            User simpleUser = new User
            {
                UserId = user.UserId,
                Name = user.Name,
                Credebility = user.Credebility,
                IdentityUserId = user.IdentityUserId,
            };
            return Ok(simpleUser);
        }

        [HttpGet("getuseridbyidentity/{id}")]
        public async Task<IActionResult> GetUserIdByIdentity(string id)
        {
            var user = await _userRepository.GetUserByIdentity(id);
            
            if (user == null)
            {
                var newUser = new User
                {
                    Name = _userManager.GetUserName(User),
                    IdentityUserId = id
                };
                await _userRepository.Create(newUser);
                user = newUser;

                //_logger.LogError("[UserController] User not found while executing" +
                //    "_userRepository.GetItemById(id)", id);
                //return NotFound("Did not find user");
            }

            //User simpleUser = new User
            //{
            //    UserId = user.UserId,
            //    Name = user.Name,
            //    Credebility = user.Credebility,
            //    IdentityUserId = user.IdentityUserId,
            //};
            return Ok(user.UserId);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(User newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Invalid user data");
            }

            bool returnOk = await _userRepository.Update(newUser);

            if (returnOk)
            {
                var response = new { success = true, message = "User " + newUser.Name + " updated succesfully" };
                return Ok(response);
            }
            else
            {
                var response = new { success = false, message = "User update failed" };
                return Ok(response);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool returnOk= await _userRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[UserController] User deletion failed for the UserId {UserId:0000}", id);
                return NotFound("User deletion failed");
            }
            var response = new { success = true, message = "User " + id.ToString()+ " deleted succesfully" };
            return Ok(response);
        }

        [HttpGet("posts/{id}")]
        public async Task<IActionResult> GetPosts(int id)
        {
            var user = await _userRepository.GetItemById(id);
            if (user == null)
            {
                _logger.LogError("[UserController] User not found while executing" +
                    "_userRepository.GetItemById(id)", id);
                return NotFound("Did not find user");
            }

            var posts = user.Posts;

            if (posts == null)
            {
                _logger.LogError("[PostController] Post list not found when executing _postRepository.GetBySubForum(),");
                return NotFound("Post list not found");
            }
            //  Create simplified post without reference to other entities to avoid referencing loop by json.
            List<Post> viewModelPosts = new List<Post>();
            foreach (var post in posts)
            {
                Post simplePost = new Post
                {
                    PostID = post.PostID,
                    Title = post.Title,
                    Text = post.Text,
                    ImageUrl = post.ImageUrl,
                    PostDate = post.PostDate,
                    //UserId = post.UserId,
                    UpvoteCount = post.UpvoteCount,
                    SubForum = post.SubForum,
                    User = new User { Name = post.User.Name },
                };
                viewModelPosts.Add(simplePost);
            }
            var postListViewModel = new PostListViewModel(viewModelPosts);
            return Ok(viewModelPosts);
        }

        [HttpGet("comments/{id}")]
        public async Task<IActionResult> GetComments(int id)
        {
            var user = await _userRepository.GetItemById(id);
            if (user == null)
            {
                _logger.LogError("[UserController] User not found while executing" +
                    "_userRepository.GetItemById(id)", id);
                return NotFound("Did not find user");
            }

            var comments = user.Comments;

            if (comments == null)
            {
                _logger.LogError("[UserController] Comment list not found when executing _postRepository.GetBySubForum(),");
                return NotFound("Post list not found");
            }
            //  Create simplified post without reference to other entities to avoid referencing loop by json.
            List<Comment> viewModelComments = new List<Comment>();
            foreach (var comment in comments)
            {
                Comment simpleComment = new Comment
                {
                    CommentID = comment.CommentID,
                    CommentText = comment.CommentText,
                    PostDate = comment.PostDate,
                    Post = new Post { Title = comment.Post.Title, PostID = comment.Post.PostID },
                };
                viewModelComments.Add(simpleComment);
            }
            return Ok(viewModelComments);
        }

        [HttpGet("votes/{id}")]
        public async Task<IActionResult> GetVotes(int id)
        {
            var user = await _userRepository.GetItemById(id);
            if (user == null)
            {
                _logger.LogError("[UserController] User not found while executing" +
                    "_userRepository.GetItemById(id)", id);
                return NotFound("Did not find user");
            }

            var votes = user.UserVotes;

            if (votes == null)
            {
                _logger.LogError("[UserController] Votes list not found when executing _postRepository.GetBySubForum(),");
                return NotFound("Post list not found");
            }
            //  Create simplified post without reference to other entities to avoid referencing loop by json.
            List<Upvote> viewModelVotes = new List<Upvote>();
            foreach (var vote in votes)
            {
                Upvote simpleVote = new Upvote
                {
                    PostID = vote.PostID,
                    Vote = vote.Vote,
                    Post = new Post { Title = vote.Post.Title, PostID = vote.Post.PostID },
                };
                viewModelVotes.Add(simpleVote);
            }
            return Ok(viewModelVotes);
        }

        //private static int GetNextItemId()
        //{
        //    if (Items.Count == 0)
        //    {
        //        return 1;
        //    }
        //    return Items.Max(item => item.ItemId) + 1;
        //}
    }
}
