using LambdaForum.Data;
using LambdaForum.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaForum.Service
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Post newPost)
        {
            _context.Add(newPost);
            await _context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditPostContent(int id, string newContent)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts
                .Include(p => p.Forum)
                .Include(p => p.User)
                .Include(p => p.Replies);
        }

        public Post GetById(int id)
        {
            return _context.Posts
                .Where(post => post.Id == id)
                .Include(post => post.User)
                .Include(post => post.Replies)
                    .ThenInclude(reply => reply.User)
                .Include(post => post.Forum)
                .FirstOrDefault();
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetLatestPosts(int numberOfPosts)
        {
            return GetAll()
                .OrderByDescending(post => post.Created)
                .Take(numberOfPosts);
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return _context.Forums
                .Where(f => f.Id == id)
                .FirstOrDefault()
                .Posts;

        }
    }
}
