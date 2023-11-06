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
    public class PostController : Controller
    {

        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<PostController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PostController(IPostRepository postRepository, IUserRepository userRepository, ILogger<PostController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //private static List<Item> Items = new List<Item>()
        //{
        //    new Item
        //    {
        //        ItemId = 1,
        //        Name = "Pizza",
        //        Price = 150,
        //        Description = "Delicouououoosoo",
        //        ImageUrl = "assets/images/pizza.jpg"
        //    },
        //    new Item
        //    {
        //        ItemId = 2,
        //        Name = "Fried Chicka",
        //        Price = 20,
        //        Description = "Crispy duck",
        //        ImageUrl = "assets/images/chickenleg.jpg"
        //    }            
        //};

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Console.Write("postcontroller1");
            var posts = await _postRepository.GetAll();
            Console.Write("postcontroller2");
            if (posts == null)
            {
                Console.Write("postcontroller3");
                _logger.LogError("[PostController] Post list not found when executing _postRepository.GetAll(),");
                return NotFound("Post list not found");
            }
            //  Create simplified post without reference to other entities to avoid referencing loop by json.
            List<Post> viewModelPosts = new List<Post>();
            foreach (var post in posts)
            {
                Post simplePost = new Post {
                    PostID = post.PostID,
                    Title = post.Title,
                    Text = post.Text,
                    ImageUrl = post.ImageUrl,
                    PostDate = post.PostDate,
                    //UserId = post.UserId,
                    UpvoteCount = post.UpvoteCount,
                    SubForum = post.SubForum,
                    User = new User { Name = post.User.Name},
                };
                viewModelPosts.Add(simplePost);
            }
            var postListViewModel = new PostListViewModel(viewModelPosts);
            return Ok(viewModelPosts);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Post newPost)
        {
            Console.WriteLine("postontroleler");
            if (newPost == null)
            {
                return BadRequest("Invalid post data");
            }

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
                    newPost.User = newUser;
                }
                else
                {
                    newPost.User = user;
                }
                //  --->
            }
            else
            {
                Console.WriteLine("----Not signed in----");
            }

            Post simplePost = new Post
            {
                PostID = newPost.PostID,
                Title = newPost.Title,
                Text = newPost.Text,
                ImageUrl = newPost.ImageUrl,
                PostDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                UserId = newPost.UserId,
                UpvoteCount = newPost.UpvoteCount,
                SubForum = "General",
                User = newPost.User
            };
            //newItem.ItemId = GetNextItemId();
            bool returnOk = await _postRepository.Create(simplePost);

            if (returnOk)
            {
                var response = new { success = true, message = "Post " + newPost.Title + " created succesfully" };
                return Ok(response);                
            }
            else
            {
                var response = new { success = false, message = "Post creation failed" };
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItembyId(int id)
        {
            var post = await _postRepository.GetItemById(id);
            if (post == null)
            {
                _logger.LogError("[PostController] Post list not found when executing _postRepository.GetAll(),");
                return NotFound("Post list not found");
            }
            Post simplePost = new Post
            {
                PostID = post.PostID,
                Title = post.Title,
                Text = post.Text,
                ImageUrl = post.ImageUrl,
                PostDate = post.PostDate,
                UserId = post.UserId,
                UpvoteCount = post.UpvoteCount,
                SubForum = post.SubForum,
                User = new User { Name = post.User.Name },
            };
            return Ok(simplePost);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Post newPost)
        {
            Console.WriteLine(newPost);
            Console.WriteLine("---" + newPost.UserId);

            if (newPost == null)
            {
                return BadRequest("Invalid post data");
            }

            var identityUserId = _userManager.GetUserId(User);
            var user = _userRepository.GetUserByIdentity(identityUserId).Result;

            newPost.User = user;

            Console.WriteLine(22);
            bool returnOk = await _postRepository.Update(newPost);
            Console.WriteLine(33);
            Console.WriteLine(returnOk);
            if (returnOk)
            {
                var response = new { success = true, message = "Post " + newPost.Title + " updated succesfully" };
                return Ok(response);
            }
            else
            {
                var response = new { success = false, message = "Post update failed" };
                return Ok(response);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            bool returnOk= await _postRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[PostController] Post deletion failed for the PostId {PostId:0000}", id);
                return NotFound("Post deletion failed");
            }
            var response = new { success = true, message = "Post " + id.ToString()+ " deleted succesfully" };
            return Ok(response);
        }

        [HttpGet("signedin/{id}")]
        public async Task<IActionResult> GetSignedIn(int id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var userspost = false;
                var post = await _postRepository.GetItemById(id);
                var identityUserId = _userManager.GetUserId(User);
                var user = _userRepository.GetUserByIdentity(identityUserId).Result;
                if (user != null)
                {
                    if (post.UserId == user.UserId)
                    {
                        userspost = true;
                    }
                }
                var response = new { success = true, message = _userManager.GetUserId(User), userspost };
                return Ok(response);
            }
            else
            {
                var response = new { success = false, message = "User not signed in" };
                return Ok(response);
            }
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
