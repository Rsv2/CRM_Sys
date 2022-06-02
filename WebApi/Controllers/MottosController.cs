using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.ContextFolder;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер мотиваторов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class MottosController : ControllerBase
    {
        public static SiteDatasContext? Context { get; set; }
        public MottosController()
        {
            Context = new SiteDatasContext();
        }
        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="motto">Текст мотиватора</param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Motto motto)
        {
            await Context.MottosEnt.AddAsync(motto);
            await Context.SaveChangesAsync();
        }
        /// <summary>
        /// Изменить
        /// </summary>
        /// <param name="getmotto">Текст мотиватора</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Motto getmotto)
        {
            Motto motto = Context.MottosEnt.ToList().Find(u => u.Id == getmotto.Id);
            motto.Text = getmotto.Text;
            Context.MottosEnt.Update(motto);
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Получить
        /// </summary>
        /// <returns>Список мотиваторов</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Motto>> Get()
        {
            return await Context.MottosEnt.ToListAsync();
        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id">ID мотиватора</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Motto motto = Context.MottosEnt.ToList().Find(u => u.Id == id);
            Context.MottosEnt.Remove(motto);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
