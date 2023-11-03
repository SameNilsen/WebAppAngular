using Microsoft.EntityFrameworkCore;
using OsloMetAngular.Models;

namespace OsloMetAngular.DAL
{
    public class PostRepository : IPostRepository
    {
        private readonly PostDbContext _db;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(PostDbContext db, ILogger<PostRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        //  Repository method for getting all posts from database.
        public async Task<IEnumerable<Post>?> GetAll()
        {
            try
            {
                return await _db.Posts.ToListAsync();  //  Gets all posts as a list.
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository posts ToListAsync() failed when GetAll(), error " +
                    "message: {e}", e.Message);
                return null;
            }
        }

        //  Method for getting all posts belonging to a subforum.
        public IEnumerable<Post>? GetBySubForum(string forum)
        {
            try
            {
                return _db.Posts.Where(x => x.SubForum == forum);  //  A query to get all posts that has the specified forum as SubForum.
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository post Where(x => x.SubForum == forum) failed when GetBySubForum" +
                    "  error message: {e}", e.Message);
                return null;
            }
        }

        //  Method for getting a single post.
        public async Task<Post?> GetItemById(int id)
        {
            try
            {
                return await _db.Posts.FindAsync(id);  //  Tries to find one item/post by id.
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository post FindAsync(id) failed when GetItemById for" +
                    " PostID {PostID:0000}, error message: {e}", id, e.Message);
                return null;
            }
        }

        //  When creating a post, this method adds it to the database.
        public async Task<bool> Create(Post post)
        {
            try
            {
                _db.Posts.Add(post);              //  Adds it.
                await _db.SaveChangesAsync();     //  Saves it.
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository post creation failed for post {@post}, error" +
                    "message: {e}", post, e.Message);
                return false;
            }
        }

        //  When updating a post, this method updates the database with the post.
        public async Task<bool> Update(Post post)
        {
            try
            {
                _db.Posts.Update(post);        //  Updates the database.
                await _db.SaveChangesAsync();  //  Saves the database.
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository post FindAsync(id) failed when updating the" +
                    " PostID {PostID:0000}, error message: {e}", post, e.Message);
                return false;
            }
        }

        //  When deleting a post, this method deletes it from the database.
        public async Task<bool> Delete(int id)
        {
            try
            {
                var post = await _db.Posts.FindAsync(id);      //  Tries to find the post.
                if (post == null)
                {
                    _logger.LogError("[PostRepository post not found for the PostID {PostID:0000}", id);
                    return false;
                }
                _db.Posts.Remove(post);                       //  Removes it from the database.
                await _db.SaveChangesAsync();                 //  Saves the database.
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[PostRepository post deletion failed for " +
                    "the PostID {PostID:0000}, error message: {e}", id, e.Message);
                return false;
            }
        }
    }
}
