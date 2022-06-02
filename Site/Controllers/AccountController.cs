using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Site.Models;
using System.Net.Http.Headers;
using System.Text;

namespace Site.Controllers
{
    public class AccountController : Controller
    {
        const string host = "https://localhost:7133/api/";

        private HttpClient httpClient = new HttpClient();
        public static string? Name { get; set; }
        public static string? Role { get; set; }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AuthenticationRequest model)
        {
            if (model.Name == null) { model.Name = ""; }
            if (model.Password == null) { model.Password = ""; }
            string url = host + "Authentication/";
            CRUD.Token = httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model),
                Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Result;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CRUD.Token);
            if (CRUD.Token != "Unauthorize")
            {
                url = host + "values";
                string json = httpClient.GetStringAsync(url).Result;
                List<string>? r = JsonConvert.DeserializeObject<List<string>>(json);
                Name = r[0];
                Role = r[1];
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Name = "";
            Role = "";
            string url = host + "Authentication/";
            httpClient.PostAsync(url, new StringContent("", Encoding.UTF8, "application/json"));
            return RedirectToAction("Index", "Home");
        }
    }
}
