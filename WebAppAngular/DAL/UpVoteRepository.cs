using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OsloMetAngular.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OsloMetAngular.DAL
{
    public class UpVoteRepository : IUpVoteRepository
    {
        private readonly PostDbContext _db;
        private readonly ILogger<UpVoteRepository> _logger;

        public UpVoteRepository(ILogger<UpVoteRepository> logger, PostDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        //  Gets all votes from the database.
        public async Task<IEnumerable<Upvote>?> GetAll()
        {
            try
            {
                return await _db.Upvotes.ToListAsync();  //  Gets all votes as a list.
            }
            catch (Exception e)
            {
                _logger.LogError("[VoteRepository] votes ToListAsync() failed when GetAll(), error " +
                    "message: {e}", e.Message);
                return null;
            }
        }

        //  Gets a vote with the given userid id.
        public async Task<Upvote?> GetVoteById(int id)
        {
            try
            {
                return await _db.Upvotes.FindAsync(id);  //  Tries to find one item/vote by id.
            }
            catch (Exception e)
            {
                _logger.LogError("[VoteRepository] vote FindAsync(id) failed when GetVoteById for " +
                    "UpvoteID {UpvoteID:0000}, error message: {e}", id, e.Message);
                return null;
            }
        }

        //  Method for getting all votes belonging to a post.
        public IEnumerable<Upvote>? GetVotesByPostId(int id)
        {
            try
            {
                return _db.Upvotes.Where(x => x.PostID == id);  //  A query to get all votes that belong to a post.
            }
            catch (Exception e)
            {
                _logger.LogError("[VoteRepository] vote Where(x => x.PostID == id) failed when GetVotesByPostId" +
                    "  error message: {e}", e.Message);
                return null;
            }
        }



        //  When creating a vote, this method adds it to the database.
        public async Task<bool> Create(Upvote upvote)
        {
            try
            {
                _db.Upvotes.Add(upvote);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[VoteRepository] vote creation failed for vote {@upvote}, error " +
                    "message: {e}", upvote, e.Message);
                return false;
            }
        }

        //  When updating a vote, this method updates the database with the edited vote.
        public async Task<bool> Update(Upvote upvote)
        {
            try
            {
                _db.Upvotes.Update(upvote);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[VoteRepository] vote FindAsync(id) failed when updating " +
                    "the UpvoteID {UpvoteID:0000}, error message: {e}", upvote, e.Message);
                return false;
            }
        }

        //  When deleting a vote, this method deletes it from the database.
        public async Task<bool> Delete(int id)
        {
            try
            {
                var upvote = await _db.Upvotes.FindAsync(id);
                if (upvote == null)
                {
                    _logger.LogError("[VoteRepository] vote not found for the UpvoteID {UpvoteID:0000}", id);
                    return false;
                }
                _db.Upvotes.Remove(upvote);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[VoteRepository] vote deletion failed for the " +
                    "UpvoteID {UpvoteID:0000}, error message: {e}", id, e.Message);
                return false;
            }
        }      
    }
}
