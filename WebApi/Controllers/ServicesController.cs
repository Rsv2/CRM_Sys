using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.ContextFolder;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер услуг
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ServicesController : ControllerBase
    {
        public static SiteDatasContext? Context { get; set; }
        public ServicesController()
        {
            Context = new SiteDatasContext();
        }
        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="service">Услуга</param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Service service)
        {
            await Context.ServicesEnt.AddAsync(service);
            await Context.SaveChangesAsync();
        }
        /// <summary>
        /// Изменить
        /// </summary>
        /// <param name="getservice">Услуга</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Service getservice)
        {
            Service service = Context.ServicesEnt.ToList().Find(u => u.Id == getservice.Id);
            service.Header = getservice.Header;
            service.Description = getservice.Description;
            Context.ServicesEnt.Update(service);
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Получить
        /// </summary>
        /// <returns>Список услуг</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Service>> Get()
        {
            return await Context.ServicesEnt.ToListAsync();
        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id">ID услуги</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Service service = Context.ServicesEnt.ToList().Find(u => u.Id == id);
            Context.ServicesEnt.Remove(service);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
