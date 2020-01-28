using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LambdaForum.Data;
using LambdaForum.Models.Forum;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForum.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;

        public ForumController(IForum forumService)
        {
            _forumService = forumService;
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
    }
}