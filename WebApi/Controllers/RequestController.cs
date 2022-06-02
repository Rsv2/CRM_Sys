using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebApi.ContextFolder;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер заявок
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class RequestController : ControllerBase
    {
        public static SiteDatasContext? Context { get; set; }

        public static List<RequestMessage> Requests = new List<RequestMessage>();

        private static HttpClient httpClient { get; set; }

        private static string BotUrl { get; set; }

        private static string PrevDatas { get; set; }

        private static int Update_id { get; set; }

        public RequestController()
        {
            Context = new SiteDatasContext();
            httpClient = new HttpClient();
            BotUrl = @"https://api.telegram.org/bot5125573757:AAHes27KOgzog3joCD_F5FuywNWfm4jCtHg/";
            Update_id = 0;
        }
        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] RequestMessage request)
        {
            request.Status = 0;
            await Context.RequestEnt.AddAsync(request);
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Изменить
        /// </summary>
        /// <param name="req">Заявка</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] RequestMessage req)
        {
            RequestMessage request = Context.RequestEnt.ToList().Find(u => u.Id == req.Id);
            request.Id = req.Id;
            request.Name = req.Name;
            request.Message = req.Message;
            request.Status = req.Status;
            Context.RequestEnt.Update(request);
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Получить
        /// </summary>
        /// <returns>Коллекция заявок</returns>
        [HttpGet]
        public async Task<IEnumerable<RequestMessage>> Get()
        {
            ReadDataFromBot();
            return await Context.RequestEnt.ToListAsync();
        }
        /// <summary>
        /// Получение заявок от бота.
        /// </summary>
        private void ReadDataFromBot()
        {
            string url = $"{BotUrl}getUpdates";
            string r = httpClient.GetStringAsync(url).Result;
            if (PrevDatas != r)
            {
                PrevDatas = r;
                var msgs = JObject.Parse(r)["result"].ToArray();

                foreach (dynamic msg in msgs)
                {
                    string userMessage = msg.message.text;
                    string userId = msg.message.from.id;
                    string useFirstrName = msg.message.from.first_name;

                    RequestMessage request = new RequestMessage(0, useFirstrName, userId, userMessage);
                    DateTime date = new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(msg.message.date));
                    request.Date = date;

                    if (Context.RequestEnt.ToList().Find(u => u.Date == request.Date && u.Name == request.Name) == null)
                    {
                        request.Status = 0;
                        Context.RequestEnt.AddAsync(request);
                        Context.SaveChangesAsync();

                        url = $"{BotUrl}sendMessage?chat_id={userId}&text=Заявка принята к рассмотрению";
                        r = httpClient.GetStringAsync(url).Result;
                    }
                }
            }
        }
    }
}
