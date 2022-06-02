using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Site.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static List<Service>? Services = new List<Service>();

        public static Service? Template { get; set; }

        public ServicesController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult ShowServices()
        {
            Services = JsonConvert.DeserializeObject<List<Service>>(CRUD.Read("Services"));
            return View();
        }

        public IActionResult AddService(int? id)
        {
            if (id != null)
            {
                Service? temp = Services.Find(u => u.Id == id);
                Template = new Service(temp.Id, temp.Header, temp.Description);
            }
            else Template = new Service();
            return RedirectToAction("ServiceEdit", "Services");
        }

        public IActionResult ServiceEdit()
        {
            return View();
        }

        public IActionResult DeleteService(string id)
        {
            CRUD.Delete($"Services/{id}");
            return RedirectToAction("ShowServices", "Services");
        }

        [HttpPost]
        public IActionResult UpdateService(string Image, string Header, string Description)
        {
            Template.Header = Header;
            Template.Description = Description;
            if (Template.Id == 0)
            {
                CRUD.Create("Services", JsonConvert.SerializeObject(Template));
            }
            else
            {
                CRUD.Update("Services", JsonConvert.SerializeObject(Template));
            }
            return RedirectToAction("ShowServices", "Services");
        }
    }
}