using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.ContextFolder;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер проектов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ProjectController : ControllerBase
    {
        public static SiteDatasContext? Context { get; set; }
        public ProjectController()
        {
            Context = new SiteDatasContext();
        }
        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="data">Проект</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Project data)
        {
            Project project = new Project(0, data.Header, data.Image, data.Description);
            await Context.ProjectsEnt.AddAsync(project);
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Изменить
        /// </summary>
        /// <param name="data">Проект</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Project data)
        {
            Project project = Context.ProjectsEnt.ToList().Find(u => u.Id == data.Id);
            project.Image = data.Image;
            project.Header = data.Header;
            project.Description = data.Description;
            Context.ProjectsEnt.Update(project);
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Получить
        /// </summary>
        /// <returns>Коллекция проектов</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Project>> Get()
        {
            return await Context.ProjectsEnt.ToListAsync();
        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id">ID проекта</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Project project = Context.ProjectsEnt.ToList().Find(u => u.Id == id);
            Context.ProjectsEnt.Remove(project);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
