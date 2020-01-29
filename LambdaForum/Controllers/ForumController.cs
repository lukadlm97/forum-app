using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LambdaForum.Data;
using LambdaForum.Data.Models;
using LambdaForum.Models.Forum;
using LambdaForum.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForum.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;

        public ForumController(IForum forumService,IPost postService)
        {
            _forumService = forumService;
            _postService = postService;
        }
        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(f => new ForumListingModel { 
                    Id = f.Id,
                    Name = f.Title,
                    Description = f.Description
                });

            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id)
        {
            var forum = _forumService.GetById(id);
            var posts = _postService.GetPostsByForum(id);

            var postListing = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                Titile = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildFormListing(post)
            });

            var model = new ForumTopicModel
            {
                Posts = postListing,
                Forum = BuildFormListing(forum)
            };

            return View(model);
        }

        private ForumListingModel BuildFormListing(Post post)
        {
            var forum = post.Forum;
            return BuildFormListing(forum);
        }

        private ForumListingModel BuildFormListing(Forum forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }
    }
}