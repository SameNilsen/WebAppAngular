using Microsoft.AspNetCore.Authorization;
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
        private readonly PostDbContext _postDbContext;

        public PostController(IPostRepository postRepository, IUserRepository userRepository, ILogger<PostController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, PostDbContext postDbContext)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _postDbContext = postDbContext;
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

        [HttpGet("subforum/{forum}")]
        public async Task<IActionResult> GetBySubForum(string forum)
        {
            var posts = _postRepository.GetBySubForum(forum);

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

        [Authorize]
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
                SubForum = newPost.SubForum,
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
                if (post == null)
                {
                    var responseO = new { success = true, message = _userManager.GetUserId(User), userspost };
                    return Ok(responseO);
                }
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

        //  A supporting method for getting the vote of a post.
        public async Task<Upvote> GetVote(Post post)
        {
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
            }
            user = _userRepository.GetUserByIdentity(identityUserId).Result;
            if (user == null)
            {
                return new Upvote { UserId = -1 };  //  Create a dummy upvote to be returned, so that the calling method handles error.
            }
            //  --->

            if (post.UserVotes != null)  //  Check to see if there are any votes on that post.
            {
                //  Finds and returns the vote.
                if (post.UserVotes.Exists(x => x.UserId == user.UserId && x.Post == post))
                {
                    var modelVote = post.UserVotes.FirstOrDefault(x => x.UserId == user.UserId && x.Post == post);
                    if (modelVote != null)
                    {
                        return modelVote;
                    }
                }
            }
            //  If the user has not voted on this post before, a new vote element is created.
            var newVote = new Upvote
            {
                UserId = user.UserId,
                User = user,
                PostID = post.PostID,
                Post = post
            };

            return newVote;
        }

        //  An action method called when a user upvotes a post.
        [Authorize]
        [HttpGet("upvote/{id}")]
        public async Task<IActionResult> UpVote(int id)
        {

            var post = await _postRepository.GetItemById(id);  //  Finds the post in question.

            if (post == null)
            {
                _logger.LogError("[PostController] Post not found for the PostID {PostID:0000}", id);
                return BadRequest("Post not found for the PostID");
            }

            var vote = GetVote(post).Result;  //  Uses the GetVote() method to see and/or get the previous vote.

            if (vote.UserId == -1)  //  If user could not be found in GetVote()
            {
                _logger.LogError("[PostController] Post not found for the PostID {PostID:0000}", id);
                return BadRequest("Post not found for the PostID");
            }

            //  If the user has upvoted the post before then nothing should happen. Else if
            //   the user has not voted or the previous vote was a downvote:
            if (vote.Vote == string.Empty || vote.Vote == "downvote")
            {
                //  If the user has downvoted before and then upvotes, then the votecount
                //   should increment by two. If this is the first vote for the user on 
                //    this post the votecount should only be increased by one.
                if (vote.Vote == "downvote") { post.UpvoteCount = post.UpvoteCount + 2; }
                else { post.UpvoteCount++; }

                await _postRepository.Update(post);  //  First update the database with the new votecount.

                vote.Vote = "upvote";  //  Set the new vote to upvote.
                _postDbContext.Upvotes.Update(vote);  // Then update the vote in the database.
                await _postDbContext.SaveChangesAsync();  //  Save it all.

                post.User.Credebility += 9;  //  When a post gets upvoted, the posts poster gets added "Credebility".
                await _userRepository.Update(post.User);
            }
            //ViewBag.Vote = "Hei herfra upp";  //  Tror ikke er i bruk??            

            //  A bug when upvoting a post from the subforum page forces us to use this method for
            //   redirecting back to the page. Explanation in documentation.
            var response = new { success = true, message = "Upvoted succesfully" };
            return Ok(response);
        }

        //  An action method called when a user downvotes a post.
        [Authorize]
        [HttpGet("downvote/{id}")]
        public async Task<IActionResult> DownVote(int id)
        {

            var post = await _postRepository.GetItemById(id);  //  Finds the post in question.

            if (post == null)
            {
                _logger.LogError("[PostController] Post not found for the PostID {PostID:0000}", id);
                return BadRequest("Post not found for the PostID");
            }

            var vote = GetVote(post).Result;  //  Uses the GetVote() method to see and/or get the previous vote.

            if (vote.UserId == -1)  //  If user could not be found in GetVote()
            {
                _logger.LogError("[PostController] Post not found for the PostID {PostID:0000}", id);
                return BadRequest("Post not found for the PostID");
            }

            //  If the user has downvoted the post before then nothing should happen. Else if
            //   the user has not voted or the previous vote was an upvote:
            if (vote.Vote == string.Empty || vote.Vote == "upvote")
            {
                //  If the user has upvoted before and then downvotes, then the votecount
                //   should decreased by two. If this is the first vote for the user on 
                //    this post the votecount should only be decreased by one.
                if (vote.Vote == "upvote") { post.UpvoteCount = post.UpvoteCount - 2; }
                else { post.UpvoteCount--; }

                await _postRepository.Update(post);  //  First update the database with the new votecount.

                vote.Vote = "downvote";  //  Set the new vote to downvote.
                _postDbContext.Upvotes.Update(vote);  // Then update the vote in the database.
                await _postDbContext.SaveChangesAsync();  //  Save it all.

                post.User.Credebility -= 4;  //  When a post gets downvoted, the posts poster loses "Credebility".
                await _userRepository.Update(post.User);
            }

            //  A bug when downvoting a post from the subforum page forces us to use this method for
            //   redirecting back to the page. Explanation in documentation.
            var response = new { success = true, message = "Downvoted succesfully" };
            return Ok(response);
        }

        // Gets vote for this post, if any.
        [HttpGet("getvote/{id}")]
        public async Task<IActionResult> GetVoteString(int id)
        {
            var vote = "blank";
            if (_signInManager.IsSignedIn(User) == false)
            {
                //  Not signed in, so no vote.
                var Nresponse = new { success = false, message = "Not signed in" };
                return Ok(Nresponse);
            }
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
            }
            user = _userRepository.GetUserByIdentity(identityUserId).Result;
            if (user == null)
            {
                _logger.LogError("[PostController] User not found for this PostID {PostID:0000}", identityUserId);
                var Nresponse = new { success = false, message = "could not get user" };
                return Ok(Nresponse);
            }
            var post = await _postRepository.GetItemById(id);

            if (post.UserVotes != null)
            {
                if (post.UserVotes.Exists(x => x.UserId == user.UserId && x.Post == post))
                {
                    var modelVote = post.UserVotes.FirstOrDefault(x => x.UserId == user.UserId && x.Post == post);
                    if (modelVote != null)
                    {
                        vote = modelVote.Vote;
                    }
                    else
                    {
                        vote = "error";
                    }
                }
                else
                {
                    vote = "blank";
                }
            }
            else { vote = "error"; }

            var response = new { success = true, vote = vote };
            return Ok(response);
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
