using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OsloMetAngular.DAL;
using OsloMetAngular.Models;

namespace OsloMetAngular.Controllers
{
    [ApiController]   //  Tags this controller as an ApiController which should handle http api requests.
    [Route("api/[controller]")]    //  This translates to 'api/post' which is the route.
    public class PostController : Controller
    {

        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUpVoteRepository _upvoteRepository;
        private readonly ILogger<PostController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PostController(IPostRepository postRepository, IUserRepository userRepository, ILogger<PostController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUpVoteRepository upvoteRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _upvoteRepository = upvoteRepository;
        }

        // Gets all posts.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postRepository.GetAll();  //  calls repo for all posts.
            
            if (posts == null)
            {
                //  If return is null, log error and return NotFoundObjectResult.
                _logger.LogError("[PostController] Post list not found when executing _postRepository.GetAll(),");
                return NotFound("Post list not found");
            }
            //  Create simplified post without reference to other entities to avoid referencing loop by json.
            //   All entities in the datbase are essentially related in some way, so when we do a
            //   getAll method like this, we are almost getting the entire database. This worked in
            //   project 1, but angular wont let us do this and if we work around the error messages, 
            //   we would see a big amount of memoryusage. So therefore we limit the amount of 
            //   information we send back to client, by excluding some of the navigational/relational
            //   properties.
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
                    User = new User { Name = post.User.Name, Credebility = post.User.Credebility, UserId = post.User.UserId},
                };
                viewModelPosts.Add(simplePost);
            }
            return Ok(viewModelPosts);
        }

        //  Creates new post.
        [Authorize]  //  Authorization is also handled in frontend.
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Post newPost)  //  Gets the newPost from request body.
        {
            if (newPost == null)
            {
                return BadRequest("Invalid post data");  //  If the post is of bad format.
            }

            if (_signInManager.IsSignedIn(User))  //  Verify that the user is logged in.
            {               
                //  <--- This block is for getting both the User user and IdentityUser user. We need the
                //       IdentityUser because then we can automatically assign the user as the 
                //        logged in user.
                var identityUserId = _userManager.GetUserId(User)!;  //  This cannot be null as we know the user is signed in and therefore there has to be a user.                
                var user = _userRepository.GetUserByIdentity(identityUserId).Result;
                if (user == null)
                {
                    var newUser = new User
                    {
                        Name = _userManager.GetUserName(User)!,  //  Again, we know this cannot be null.
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
            //  Create a new post with the patched values + the newly computed User.
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

            bool returnOk = await _postRepository.Create(simplePost);  //  Try to create post via repo.

            if (returnOk)
            {
                //  If success:
                simplePost.User.Credebility += 7;  //  When creating a post the user gets added score of 7 to their "Credebility".
                await _userRepository.Update(simplePost.User);  //  Update for creds.
                var response = new { success = true, message = "Post " + newPost.Title + " created succesfully" };
                return Ok(response);  //  Return a response.               
            }
            else
            {
                //  If fail, return response message with error parameters.
                var response = new { success = false, message = "Post creation failed" };
                return Ok(response);
            }
        }

        //  Fetches a spesific post by a given postId.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItembyId(int id)
        {
            var post = await _postRepository.GetItemById(id);  //  Get post from repo.
            if (post == null)
            {
                _logger.LogError("[PostController] Post list not found when executing _postRepository.GetAll(),");
                return NotFound("Post list not found");
            }
            //  Create a simplified post because we dont want all relational entities.
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

        //  Updates a post.
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Post newPost)
        {

            if (newPost == null)
            {
                return BadRequest("Invalid post data");
            }

            //  Only a logged in user can update a post. This is verified in the frontend. We 
            //   therefore know the user and can get it with this:
            var identityUserId = _userManager.GetUserId(User)!;
            var user = _userRepository.GetUserByIdentity(identityUserId).Result!;
            //  To be able to update a post, you must first own the post, and to own the post
            //   you must have created the post. When creating a post it is checked or created
            //   that the User entity is linked with IdentityUser so we know that it exists a
            //   user with the given identityUserId.
            newPost.User = user;

            bool returnOk = await _postRepository.Update(newPost);  //  Call on repo to update post.
            
            if (returnOk)  //  Success:
            {
                var response = new { success = true, message = "Post " + newPost.Title + " updated succesfully" };
                return Ok(response);
            }
            else  //  Fail:
            {
                var response = new { success = false, message = "Post update failed" };
                return Ok(response);
            }
        }

        //  Deletes a post.
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            bool returnOk= await _postRepository.Delete(id);  //  Call on repo to delete.
            if (!returnOk)  //  On fail:
            {
                _logger.LogError("[PostController] Post deletion failed for the PostId {PostId:0000}", id);
                return NotFound("Post deletion failed");
            }
            //  On success:
            var response = new { success = true, message = "Post " + id.ToString()+ " deleted succesfully" };
            return Ok(response);
        }

        //  Fetches all posts that belongs to a given subforum.
        [HttpGet("subforum/{forum}")]
        public IActionResult GetBySubForum(string forum)
        {
            var posts = _postRepository.GetBySubForum(forum);  //  Calls on repo for posts.

            if (posts == null)  //  On error.
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
            //  Return the revised list of posts.
            return Ok(viewModelPosts);
        }

        //  Finds out if a user is signed in. It takes in a postId as paramater so it can check if 
        //   a post belongs to the logged in user. It can return a success string saying if the user 
        //   is logged in, a message string with the logged in identityUserId and a userspost string
        //   which says wether or not a post belongs to the logged in user.
        [HttpGet("signedin/{id}")]
        public async Task<IActionResult> GetSignedIn(int id)
        {
            if (_signInManager.IsSignedIn(User))  //  Check if logged in, if true continue.
            {
                var userspost = false;  //  Default not the users post.
                var post = await _postRepository.GetItemById(id);  //  Get the post.
                if (post == null)  //  No post = logged in but not userspost.
                {
                    var responseO = new { success = true, message = _userManager.GetUserId(User), userspost };
                    return Ok(responseO);
                }
                //  If we find the post, we must get the user and check if the post belong to it.
                var identityUserId = _userManager.GetUserId(User)!;
                var user = _userRepository.GetUserByIdentity(identityUserId).Result;
                if (user != null)
                {
                    if (post.UserId == user.UserId)
                    {
                        userspost = true;  //  The post is the user's!
                    }
                }
                //  Return that the user is logged in and that the post is its own.
                var response = new { success = true, message = _userManager.GetUserId(User), userspost };
                return Ok(response);
            }
            else
            {
                //  Not logged in:
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
            var identityUserId = _userManager.GetUserId(User)!;
            var user = _userRepository.GetUserByIdentity(identityUserId).Result;
            if (user == null)
            {
                var newUser = new User
                {
                    Name = _userManager.GetUserName(User)!,
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

                //_postDbContext.Upvotes.Update(vote);  // Then update the vote in the database.
                //await _postDbContext.SaveChangesAsync();  //  Save it all.
                await _upvoteRepository.Update(vote);   //  Use repo instead to save.

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
                //_postDbContext.Upvotes.Update(vote);  // Then update the vote in the database.
                //await _postDbContext.SaveChangesAsync();  //  Save it all.
                await _upvoteRepository.Update(vote);   //  Use repo instead to save.

                post.User.Credebility -= 4;  //  When a post gets downvoted, the posts poster loses "Credebility".
                await _userRepository.Update(post.User);
            }

            //  A bug when downvoting a post from the subforum page forces us to use this method for
            //   redirecting back to the page. Explanation in documentation.
            var response = new { success = true, message = "Downvoted succesfully" };
            return Ok(response);
        }

        // Gets vote for a given post, if any.
        [HttpGet("getvote/{id}")]
        public async Task<IActionResult> GetVoteString(int id)
        {
            var vote = "blank";  //  Default no vote.
            if (_signInManager.IsSignedIn(User) == false)
            {
                //  Not signed in, so no vote.
                var Nresponse = new { success = false, message = "Not signed in" };
                return Ok(Nresponse);
            }
            //  Find the user.
            var identityUserId = _userManager.GetUserId(User)!;
            var user = _userRepository.GetUserByIdentity(identityUserId).Result;
            if (user == null)
            {
                //  No User but logged in? -> Create user.
                var newUser = new User
                {
                    Name = _userManager.GetUserName(User)!,
                    IdentityUserId = identityUserId
                };
                await _userRepository.Create(newUser);
            }
            user = _userRepository.GetUserByIdentity(identityUserId).Result;
            if (user == null)
            {
                //  No user. Log error, respond with error string.
                _logger.LogError("[PostController] User not found for this PostID {PostID:0000}", identityUserId);
                var Nresponse = new { success = false, message = "could not get user" };
                return Ok(Nresponse);
            }
            var post = await _postRepository.GetItemById(id);  //  Get the post in question.
            if (post == null)
            {
                var Nresponse = new { success = false, message = "Could not find post" };
                return Ok(Nresponse);
            }

            //  Find the vote.
            if (post.UserVotes != null)
            {
                if (post.UserVotes.Exists(x => x.UserId == user.UserId && x.Post == post))
                {
                    var modelVote = post.UserVotes.FirstOrDefault(x => x.UserId == user.UserId && x.Post == post);
                    if (modelVote != null)
                    {
                        vote = modelVote.Vote;  //  Save the vote.
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
            //  Return response with the vote.
            var response = new { success = true, vote = vote };
            return Ok(response);
        }       
    }
}
