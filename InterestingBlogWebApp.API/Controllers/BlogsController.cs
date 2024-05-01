using Microsoft.AspNetCore.Mvc;

namespace InterestingBlogWebApp.API.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
