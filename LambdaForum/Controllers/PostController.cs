using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LambdaForum.Data;
using LambdaForum.Data.Models;
using LambdaForum.Models.Post;
using LambdaForum.Models.Reply;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForum.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postsService;
        private readonly IForum _forumService;
        private static UserManager<ApplicationUser> _userManager;

        public PostController(IPost postsService,IForum forumService,UserManager<ApplicationUser> userManager)
        {
            _postsService = postsService;
            _forumService = forumService;
            _userManager = userManager;
        }
        public IActionResult Index(int id)
        {
            var post = _postsService.GetById(id);
            var replies = BuildPostReplies(post.Replies);

            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName  = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                AuthorRating = post.User.Rating,
                Created = post.Created,
                PostContent = post.Content,
                Repies = replies
            };

            return View(model);
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                Created = reply.Created,
                ReplyContent = reply.Content
            });
        }


        public IActionResult Create(int id)
        {
            var forum = _forumService.GetById(id);

            var model = new NewPostModel
            {
                ForumName = forum.Title,
                ForumId = forum.Id,
                ForumImageUrl = forum.ImageUrl,
                AuthorName = User.Identity.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var post = BuildPost(model, user);

            await _postsService.Add(post);

            return RedirectToAction("Index", "Post",post.Id);
        }

        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user
            };
        }
    }
}