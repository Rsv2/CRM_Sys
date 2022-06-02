using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Site.Models;

namespace Site.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static List<Project>? Blogs = new List<Project>();

        public static Project? Template { get; set; }

        public BlogController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult ShowBlog()
        {
            Blogs = JsonConvert.DeserializeObject<List<Project>>(CRUD.Read("Blog"));
            return View();
        }

        public IActionResult AddBlog(int? id)
        {
            ImageController.Pics = JsonConvert.DeserializeObject<List<string>>(CRUD.Read("Pics"));
            if (id != null)
            {
                Project? temp = Blogs.Find(u => u.Id == id);
                Template = new Project(temp.Id, temp.Header, temp.Image, temp.Description);
            }
            else Template = new Project();
            return RedirectToAction("BlogEdit", "Blog");
        }

        public IActionResult BlogEdit()
        {
            return View();
        }

        public IActionResult DeleteBlog(string id)
        {
            CRUD.Delete($"Blog/{id}");
            return RedirectToAction("ShowBlog", "Blog");
        }

        [HttpPost]
        public IActionResult UpdateBlog(string Image, string Header, string Description)
        {
            Template.Image = Image;
            Template.Header = Header;
            Template.Description = Description;
            if (Template.Id == 0)
            {
                CRUD.Create("Blog", JsonConvert.SerializeObject(Template));
            }
            else
            {
                CRUD.Update("Blog", JsonConvert.SerializeObject(Template));
            }
            return RedirectToAction("ShowBlog", "Blog");
        }
    }
}
