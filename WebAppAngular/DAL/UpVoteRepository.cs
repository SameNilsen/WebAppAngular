﻿using Microsoft.EntityFrameworkCore;
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

        //  Gets all comments from the database.
        public async Task<IEnumerable<Upvote>?> GetAll()
        {
            try
            {
                return await _db.Upvotes.ToListAsync();  //  Gets all comments as a list.
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] comments ToListAsync() failed when GetAll(), error " +
                    "message: {e}", e.Message);
                return null;
            }
        }

        //  Gets a comment with the given userid id.
        public async Task<Upvote?> GetVoteById(int id)
        {
            try
            {
                return await _db.Upvotes.FindAsync(id);  //  Tries to find one item/comment by id.
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] comment FindAsync(id) failed when GetItemById for " +
                    "CommentID {CommentID:0000}, error message: {e}", id, e.Message);
                return null;
            }
        }

        //  Method for getting all comments belonging to a post.
        public IEnumerable<Upvote>? GetVotesByPostId(int id)
        {
            try
            {
                return _db.Upvotes.Where(x => x.PostID == id);  //  A query to get all posts that has the specified forum as SubForum.
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
        public async Task<bool> Create(Upvote upvote)
        {
            try
            {
                Console.WriteLine("COMMENTREPOO----");
                _db.Upvotes.Add(upvote);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[CommentRepository] comment creation failed for comment {@comment}, error " +
                    "message: {e}", upvote, e.Message);
                return false;
            }
        }

        //  When updating a comment, this method updates the database with the edited comment.
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
                _logger.LogError("[CommentRepository] comment FindAsync(id) failed when updating " +
                    "the CommentID {CommentID:0000}, error message: {e}", upvote, e.Message);
                return false;
            }
        }

        //  When deleting a comment, this method deletes it from the database.
        public async Task<bool> Delete(int id)
        {
            try
            {
                var upvote = await _db.Upvotes.FindAsync(id);
                if (upvote == null)
                {
                    _logger.LogError("[CommentRepository] comment not found for the CommentID {CommentID:0000}", id);
                    return false;
                }
                _db.Upvotes.Remove(upvote);
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
