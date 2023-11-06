using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentRepository commentRepository, ILogger<CommentController> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
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
            if (newComment == null)
            {
                return BadRequest("Invalid comment data");
            }
            bool returnOk = await _commentRepository.Create(newComment);

            if (returnOk)
            {
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
                    User = new User { Name = comment.User.Name, Credebility = comment.User.Credebility },
                };
                viewModelComments.Add(simpleComment);
            }
            //Console.WriteLine(viewModelComments[0]. + "---");
            return Ok(viewModelComments);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Comment newComment)
        {
            if (newComment == null)
            {
                return BadRequest("Invalid comment data");
            }

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
