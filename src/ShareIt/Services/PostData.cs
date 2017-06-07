using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShareIt.Entities;

namespace ShareIt.Services
{
    public interface IPostData
    {
        IEnumerable<Post> GetAll();
        Post Get(int id);
        Post Add(Post newPost);
        void Commit();
        IEnumerable<Post> GetByPoster(User poster);
    }

    public class SQLPostData : IPostData
    {
        private ShareItDbContext _dbContext;

        public SQLPostData(ShareItDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Post> GetAll()
        {
            return _dbContext.Posts.Include(p => p.Poster);
        }

        public Post Get(int id)
        {
            return _dbContext.Posts.Include(p => p.Poster).FirstOrDefault(r => r.Id == id);
        }

        public Post Add(Post newPost)
        {
            _dbContext.Add(newPost);

            return newPost;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public IEnumerable<Post> GetByPoster(User poster)
        {
            return _dbContext.Posts.Include(p => p.Poster).Where(p => p.Poster != null && p.Poster.Id == poster.Id);
        }
    }

    public class InMemoryPostData : IPostData
    {
        private static List<Post> _posts;

        static InMemoryPostData()
        {
            _posts = new List<Post>
            {
                new Post {Id = 1, Title = "Why do cats bork?"},
                new Post {Id = 2, Title = "Do cows even meow tho?"},
                new Post {Id = 3, Title = "Why sheep beep beep like sheep"}
            };
        }

        public IEnumerable<Post> GetAll()
        {
            return _posts;
        }

        public Post Get(int id)
        {
            return _posts.FirstOrDefault(r => r.Id == id);
        }

        public Post Add(Post newPost)
        {
            newPost.Id = _posts.Max(r => r.Id) + 1;
            _posts.Add(newPost);

            return newPost;
        }

        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Post> GetByPoster(User poster)
        {
            throw new System.NotImplementedException();
        }
    }
}