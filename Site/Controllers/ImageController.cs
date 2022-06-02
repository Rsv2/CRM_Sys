using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Site.Controllers
{
    public class ImageController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static List<string>? Pics { get; set; }

        public ImageController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Pics = JsonConvert.DeserializeObject<List<string>>(CRUD.Read("Pics"));
            return View();
        }
        [HttpPost]
        public IActionResult UploadPic(IFormFileCollection images)
        {
            CRUD.Create("Pics/file", images);
            return RedirectToAction("Index", "Image");
        }
        [HttpPost]
        public IActionResult DeletePic(string id)
        {
            CRUD.Delete($"Pics/{id}");
            return RedirectToAction("Index", "Image");
        }

    }
}
