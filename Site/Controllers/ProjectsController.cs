using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Site.Models;

namespace Site.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static List<Project>? Projects = new List<Project>();

        public static Project? Template { get; set; }

        public ProjectsController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult ShowProjects()
        {
            Projects = JsonConvert.DeserializeObject<List<Project>>(CRUD.Read("Project"));
            return View();
        }

        public IActionResult AddProject(int? id)
        {
            ImageController.Pics = JsonConvert.DeserializeObject<List<string>>(CRUD.Read("Pics"));
            if (id != null)
            {
                Project? temp = Projects.Find(u => u.Id == id);
                Template = new Project(temp.Id, temp.Header, temp.Image, temp.Description);
            }
            else Template = new Project();
            return RedirectToAction("ProjectEdit", "Projects");
        }

        public IActionResult ProjectEdit()
        {
            return View();
        }

        public IActionResult DeleteProject(string id)
        {
            CRUD.Delete($"Project/{id}");
            return RedirectToAction("ShowProjects", "Projects");
        }

        [HttpPost]
        public IActionResult UpdateProject(string Image, string Header, string Description)
        {
            Template.Image = Image;
            Template.Header = Header;
            Template.Description = Description;
            if (Template.Id == 0)
            {
                CRUD.Create("Project", JsonConvert.SerializeObject(Template));
            }
            else
            {
                CRUD.Update("Project", JsonConvert.SerializeObject(Template));
            }
            return RedirectToAction("ShowProjects", "Projects");
        }
    }
}
