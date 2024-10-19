using Microsoft.AspNetCore.Mvc;
using sustainbean.Models;
using System.Diagnostics;

namespace sustainbean.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddBlogs()
        {
            return View();
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AddTags()
        {
            return View();
        }
        public IActionResult AddImages()
        {
            return View();
        }
        public IActionResult ViewBlogs()
        {
            return View();
        }
        public IActionResult ViewCategory()
        {
            return View();
        }
        public IActionResult ViewTags()
        {
            return View();
        }

        public IActionResult ViewImages()
        {
            return View();
        }
       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
