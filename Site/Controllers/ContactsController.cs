using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Site.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ILogger<ContactsController> _logger;

        public static List<Service>? Contacts = new List<Service>();
        public static Service? Selected { get; set; }

        public static Service? Template { get; set; }

        public ContactsController(ILogger<ContactsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Contacts = JsonConvert.DeserializeObject<List<Service>>(CRUD.Read("Contacts"));
            return View();
        }

        public IActionResult UpdateContact(int id, string header, string description)
        {
            CRUD.Update("Contacts", JsonConvert.SerializeObject(new Service(id, header, description)));
            return RedirectToAction("Index", "Contacts");
        }

        public IActionResult AddContact(string header, string description)
        {
            CRUD.Create("Contacts", JsonConvert.SerializeObject(new Service(0, header, description)));
            return RedirectToAction("Index", "Contacts");
        }

        public IActionResult Delete(int id)
        {
            CRUD.Delete($"Contacts/{id}");
            return RedirectToAction("Index", "Contacts");
        }
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                Selected = Contacts.Find(u => u.Id == id);
            }
            else
            {
                Selected = new Service(0, "", "");
            }
            return View();
        }
        [HttpPost]
        public IActionResult UpdateContact(IFormFile input, Service contact)
        {
            if (Selected.Id == 0 && contact.Description != "" && contact.Description != null)
            {
                CRUD.CreateContact($"Contacts/file", input, contact.Description);
            }
            else
            {
                if (input != null) CRUD.UpdateContact($"Contacts/file?id={Selected.Id}&description={contact.Description}", input);
                else CRUD.UpdateContact($"Contacts?id={Selected.Id}&description={contact.Description}");
            }
            return RedirectToAction("Index", "Contacts");
        }
    }
}
