using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using OsloMetAngular.DAL;
using OsloMetAngular.Models;
using OsloMetAngular.ViewModels;

namespace OsloMetAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {

        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<CommentController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CommentController(ICommentRepository commentRepository, IUserRepository userRepository, ILogger<CommentController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _postRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAll();
            if (comments == null)
            {
                _logger.LogError("[CommentController] Comment list not found when executing _commentRepository.GetAll(),");
                return NotFound("Comment list not found");
            }
            //  Wrap it in viewmodel without reference to other entities to avoid referencing loop by json.
            //List<Post> viewModelPosts = new List<Post>();
            //foreach (var post in posts)
            //{
            //    Post simplePost = new Post {
            //        PostID = post.PostID,
            //        Title = post.Title,
            //        Text = post.Text,
            //        ImageUrl = post.ImageUrl,
            //        PostDate = post.PostDate,
            //        //UserId = post.UserId,
            //        UpvoteCount = post.UpvoteCount,
            //        SubForum = post.SubForum,
            //        User = new User { Name = post.User.Name},
            //    };
            //    viewModelPosts.Add(simplePost);
            //}
            //var postListViewModel = new PostListViewModel(viewModelPosts);
            return Ok(comments);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Comment newComment)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Console.WriteLine("--------signed in----------");
                Console.WriteLine(_userManager.GetUserId(User));
                //  <--- This block is for getting both the User user and IdentityUser user. We need the
                //       IdentityUser because then we can automatically assign the user as the 
                //        logged in user.
                var identityUserId = _userManager.GetUserId(User);
                var user = _userRepository.GetUserByIdentity(identityUserId).Result;
                if (user == null)
                {
                    var newUser = new User
                    {
                        Name = _userManager.GetUserName(User),
                        IdentityUserId = identityUserId
                    };
                    await _userRepository.Create(newUser);
                    newComment.User = newUser;
                }
                else
                {
                    newComment.User = user;
                }
                //  --->
            }
            else
            {
                Console.WriteLine("----Not signed in----");
            }
                
            
            if (newComment == null)
            {
                return BadRequest("Invalid comment data");
            }

            Console.WriteLine(newComment);
            Console.WriteLine(newComment.ToString());
            Console.WriteLine(newComment.UserId + "..a.a.a.");
            var newComment2 = new Comment
            {
                CommentText = newComment.CommentText,
                PostDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                UserId = newComment.User.UserId,
                User = newComment.User,  //  Need to get proper user.
                PostID = newComment.PostID,
                //Post = newComment.Post
            };
            bool returnOk = await _commentRepository.Create(newComment2);

            if (returnOk)
            {
                //  Set credibility for the commenter:
                newComment2.User.Credebility += 3;
                await _userRepository.Update(newComment2.User);

                //  Set credibility for the posts poster:
                var post = await _postRepository.GetItemById(newComment2.PostID);
                if (post != null)
                {
                    //  If it cannot find the post, the poster will not get creds, which is not
                    //   detrimental.
                    post.User.Credebility += 5;
                    await _userRepository.Update(post.User);
                }
                var response = new { success = true, message = "Comment " + newComment.CommentID + " created succesfully" };
                return Ok(response);                
            }
            else
            {
                var response = new { success = false, message = "COmment creation failed" };
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentbyId(int id)
        {
            var comment = await _commentRepository.GetCommentById(id);
            if (comment == null)
            {
                _logger.LogError("[CommentController] Comment list not found when executing _commentRepository.GetAll(),");
                return NotFound("Comment list not found");
            }
            return Ok(comment);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetCommentsbyPostId(int id)
        {
            var comments = _commentRepository.GetCommentsByPostId(id);
            if (comments == null)
            {
                _logger.LogError("[CommentController] Comment list not found when executing _commentRepository.GetAll(),");
                return NotFound("Comment list not found");
            }
            //  Wrap it in viewmodel without reference to other entities to avoid referencing loop by json.
            List<Comment> viewModelComments = new List<Comment>();
            foreach (var comment in comments)
            {
                Comment simpleComment = new Comment
                {
                    CommentID = comment.CommentID,
                    CommentText = comment.CommentText,
                    PostDate = comment.PostDate,
                    UserId = comment.UserId,
                    User = new User { Name = comment.User.Name, Credebility = comment.User.Credebility, IdentityUserId = comment.User.IdentityUserId },
                };
                viewModelComments.Add(simpleComment);
            }
            //Console.WriteLine(viewModelComments[0]. + "---");
            return Ok(viewModelComments);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Comment newComment)
        {
            Console.WriteLine(newComment + "aaaaaa");
            if (newComment == null)
            {
                return BadRequest("Invalid comment data");
            }
            var post = _postRepository.GetItemById(newComment.PostID).Result!;
            newComment.Post = post;

            var identityUserId = _userManager.GetUserId(User);
            var user = _userRepository.GetUserByIdentity(identityUserId).Result;
            Console.WriteLine("User " + user.Name + " " + user.UserId);
            newComment.UserId = user.UserId;
            newComment.User = user;
            Console.WriteLine("User " + newComment.User.Name + " " + newComment.User.UserId);

            bool returnOk = await _commentRepository.Update(newComment);

            if (returnOk)
            {
                var response = new { success = true, message = "Comment " + newComment.CommentID + " updated succesfully" };
                return Ok(response);
            }
            else
            {
                var response = new { success = false, message = "Comment update failed" };
                return Ok(response);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            bool returnOk= await _commentRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[CommentController] Comment deletion failed for the CommentId {CommentId:0000}", id);
                return NotFound("Comment deletion failed");
            }
            var response = new { success = true, message = "Comment " + id.ToString()+ " deleted succesfully" };
            return Ok(response);
        }      
    }
}
