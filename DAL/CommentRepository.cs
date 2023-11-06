using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OsloMetAngular.Models;

namespace OsloMetAngular.DAL
{
    public class CommentRepository : ICommentRepository
    {
        private readonly PostDbContext _db;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(ILogger<CommentRepository> logger, PostDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        //  Gets all comments from the database.
        public async Task<IEnumerable<Comment>?> GetAll()
        {
            try
            {
                return await _db.Comments.ToListAsync();  //  Gets all comments as a list.
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] comments ToListAsync() failed when GetAll(), error " +
                    "message: {e}", e.Message);
                return null;
            }
        }

        //  Gets a comment with the given userid id.
        public async Task<Comment?> GetCommentById(int id)
        {
            try
            {
                return await _db.Comments.FindAsync(id);  //  Tries to find one item/comment by id.
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] comment FindAsync(id) failed when GetItemById for " +
                    "CommentID {CommentID:0000}, error message: {e}", id, e.Message);
                return null;
            }
        }

        //  Method for getting all comments belonging to a post.
        public IEnumerable<Comment>? GetCommentsByPostId(int id)
        {
            try
            {
                return _db.Comments.Where(x => x.PostID == id);  //  A query to get all posts that has the specified forum as SubForum.
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] comment Where(x => x.PostID == id) failed when GetCommentByPostId" +
                    "  error message: {e}", e.Message);
                return null;
            }
        }

        //  Gets the user from the database that has the IdentityUserId matching the given id.
        //public async Task<User?> GetUserByIdentity(string id)
        //{            
        //    return await _db.Users.FirstOrDefaultAsync(x => x.IdentityUserId == id);
        //}

        //  When creating a comment, this method adds it to the database.
        public async Task<bool> Create(Comment comment)
        {
            try
            {
                Console.WriteLine("COMMENTREPOO----");
                _db.Comments.Add(comment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] comment creation failed for comment {@comment}, error " +
                    "message: {e}", comment, e.Message);
                return false;
            }
        }

        //  When updating a comment, this method updates the database with the edited comment.
        public async Task<bool> Update(Comment comment)
        {
            try
            {
                _db.Comments.Update(comment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] comment FindAsync(id) failed when updating " +
                    "the CommentID {CommentID:0000}, error message: {e}", comment, e.Message);
                return false;
            }
        }

        //  When deleting a comment, this method deletes it from the database.
        public async Task<bool> Delete(int id)
        {
            try
            {
                var comment = await _db.Comments.FindAsync(id);
                if (comment == null)
                {
                    _logger.LogError("[CommentRepository] comment not found for the CommentID {CommentID:0000}", id);
                    return false;
                }
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] comment deletion failed for the " +
                    "CommentID {CommentID:0000}, error message: {e}", id, e.Message);
                return false;
            }
        }      
    }
}
