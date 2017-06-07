using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShareIt.Entities;

namespace ShareIt.Services
{
    public interface ICommentData
    {
        Comment Add(Comment newComment);
        void Commit();
        IEnumerable<Comment> GetAll();
        IEnumerable<Comment> GetByPost(int postId);
        IEnumerable<Comment> GetByCommenter(User commenter);

    }

    public class CommentData : ICommentData
    {
        private ShareItDbContext _dbContext;

        public CommentData(ShareItDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public Comment Add(Comment newComment)
        {
            _dbContext.Add(newComment);

            return newComment;
        }

        public IEnumerable<Comment> GetByPost(int id)
        {
            return _dbContext.Comments.Include(c => c.Post).Include(c => c.Commenter).Where(c => c.Commenter.Id != null && c.Post.Id == id);
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public IEnumerable<Comment> GetAll()
        {
            return _dbContext.Comments.Include(c => c.Commenter);
        }

        public IEnumerable<Comment> GetByCommenter(User commenter)
        {
            return _dbContext.Comments.Include(c => c.Post).Where(c => c.Commenter.Id != null && c.Commenter.Id == commenter.Id);
        }
    }
}
