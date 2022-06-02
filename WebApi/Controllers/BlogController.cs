using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.ContextFolder;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер управления блогом.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public static SiteDatasContext? Context { get; set; }
        /// <summary>
        /// Инициализация контроллера.
        /// </summary>
        public BlogController()
        {
            Context = new SiteDatasContext();
        }
        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="data">Запись в блоге</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Blog data)
        {
            Blog blog = new Blog(0, data.Header, data.Image, data.Description);
            await Context.BlogsEnt.AddAsync(blog);
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Изменить
        /// </summary>
        /// <param name="data">Запись в блоге</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Blog data)
        {
            Blog blog = Context.BlogsEnt.ToList().Find(u => u.Id == data.Id);
            blog.Image = data.Image;
            blog.Header = data.Header;
            blog.Description = data.Description;
            Context.BlogsEnt.Update(blog); 
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Получить
        /// </summary>
        /// <returns>Коллекция записей в блоге</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Blog>> Get()
        {
            return await Context.BlogsEnt.ToListAsync();
        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id">ID Записи в блоге</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = Context.BlogsEnt.ToList().Find(u => u.Id == id);
            Context.BlogsEnt.Remove(blog);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
