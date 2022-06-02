using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Site.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using System.Globalization;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static List<Project>? Blogs = new List<Project>();
        public static List<Service>? Services = new List<Service>();
        public static List<Motto>? Contacts = new List<Motto>();

        private HttpClient httpClient = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Отправка заявки по API.
        /// </summary>
        /// <param name="name">Имя отправителя</param>
        /// <param name="email">email отправителя</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public IActionResult Sendrequest(string name, string email, string message)
        {
            RequestMessage request = new RequestMessage(0, name, email, message);
            string url = @"https://localhost:7133/api/Request";
            string r = httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")).Result.ToString();
            Console.WriteLine(r);
            if (r.Contains("200"))
            {
                return RedirectToAction("RequestDone", "Home");
            }
            else
            {
                return RedirectToAction("RequestError", "Home");
            }
        }
        /// <summary>
        /// Заявка принята.
        /// </summary>
        /// <returns>Страница</returns>
        public IActionResult RequestDone()
        {
            return View();
        }
        /// <summary>
        /// Ошибка отправки заявки.
        /// </summary>
        /// <returns>Страница</returns>
        public IActionResult RequestError()
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