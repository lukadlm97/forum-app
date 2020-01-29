using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LambdaForum.Data;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForum.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postsService;

        public PostController(IPost postsService)
        {
            _postsService = postsService;
        }
        public IActionResult Index(int id)
        {
            var post = _postsService.GetById(id);

            return View();
        }
    }
}