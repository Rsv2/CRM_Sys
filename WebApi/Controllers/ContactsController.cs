using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.ContextFolder;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер контактов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ContactsController : ControllerBase
    {
        private IWebHostEnvironment _appEnvironment { get; set; }
        public static SiteDatasContext? Context { get; set; }
        public ContactsController(IWebHostEnvironment appEnvironment)
        {
            Context = new SiteDatasContext();
            _appEnvironment = appEnvironment;
        }
        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="file">Файл иконки</param>
        /// <param name="url">Url контакта</param>
        /// <returns></returns>
        [HttpPost, Route("file")]
        public async Task<IActionResult> Post(IFormFile file, [FromForm] string url)
        {
            if (file != null)
            {
                string name = GetName($"{Guid.NewGuid()}{file.FileName.Substring(file.FileName.LastIndexOf("."))}");
                string path = $"{_appEnvironment.WebRootPath}\\icons\\{name}";
                
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                Context.ContactsEnt.Add(new Contact(0, name, url));
                await Context.SaveChangesAsync();
            }
            return Ok();
        }
        /// <summary>
        /// Изменить
        /// </summary>
        /// <param name="file">Файл иконки</param>
        /// <param name="id">Идентификатор</param>
        /// <param name="description">Url контакта</param>
        /// <returns></returns>
        [HttpPut, Route("file")]
        public async Task<IActionResult> Put(IFormFile file, int id, string description)
        {
            if (file != null)
            {
                Contact contact = Context.ContactsEnt.ToList().Find(u => u.Id == id);
                DeletePic($"{_appEnvironment.WebRootPath}\\icons\\{contact.Header}");
                contact.Description = description;
                contact.Header = GetName(file.FileName);
                string path = $"{_appEnvironment.WebRootPath}\\icons\\{contact.Header}";
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                Context.ContactsEnt.Update(contact);
                await Context.SaveChangesAsync();
            }
            return Ok();
        }
        /// <summary>
        /// Изменить
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="description">Url контакта</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(int id, string description)
        {
            Contact contact = Context.ContactsEnt.ToList().Find(u => u.Id == id);
            contact.Description = description;
            Context.ContactsEnt.Update(contact);
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Получить
        /// </summary>
        /// <returns>Список контактов</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Contact>> Get()
        {
            return await Context.ContactsEnt.ToListAsync();
        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Contact contact = Context.ContactsEnt.ToList().Find(u => u.Id == id);
            DeletePic($"{_appEnvironment.WebRootPath}\\icons\\{contact.Header}");
            Context.ContactsEnt.Remove(contact);
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Получить имя
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetName(string name)
        {
            string path = $"{_appEnvironment.WebRootPath}\\icons\\{name}";
            while (System.IO.File.Exists(path))
            {
                string patch = $"{path.Substring(0, path.LastIndexOf("."))}~";
                path = patch + path.Substring(path.LastIndexOf("."));
            }
            return path.Substring(path.LastIndexOf("\\") + 1);
        }
        /// <summary>
        /// Удалить иконку с сервера
        /// </summary>
        /// <param name="path"></param>
        private void DeletePic(string path)
        {
            try
            {
                System.IO.File.Delete(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
