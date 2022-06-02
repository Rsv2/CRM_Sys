using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace WebApi.Controllers
{
    /// <summary>
    /// Управление изображениями для Блога и Проектов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class PicsController : ControllerBase
    {
        IWebHostEnvironment _appEnvironment;

        public PicsController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        /// <summary>
        /// Добавть
        /// </summary>
        /// <param name="file">Изображение</param>
        /// <returns></returns>
        [HttpPost, Route("file")]
        public async Task<IActionResult> Post(List<IFormFile> file)
        {
            if (file != null)
            {
                string path = "";
                for (int i = 0; i < file.Count; i++)
                {
                    path = GetName(file[i].FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file[i].CopyToAsync(fileStream);
                    }
                }
            }
            return Ok();
        }
        /// <summary>
        /// Получить
        /// </summary>
        /// <returns>Коллекция Url изображений</returns>
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<string> Get()
        {
            string Host = "https://localhost:7133";
            string[] files = Directory.GetFiles($"{_appEnvironment.WebRootPath}\\images");
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace($"{_appEnvironment.WebRootPath}", Host);
                files[i] = files[i].Replace("\\", "/");
            }
            return files;
        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="pic">URL изображения</param>
        /// <returns></returns>
        [HttpDelete, Route("{pic}")]
        public bool Delete(string pic)
        {
            string fullPath = $"{_appEnvironment.WebRootPath}\\images\\{pic}";
            if (!System.IO.File.Exists(fullPath)) 
            {
                return false;
            }
            try
            {
                System.IO.File.Delete(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }
        /// <summary>
        /// Получить имя
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetName(string name)
        {
            string path = $"{_appEnvironment.WebRootPath}\\images\\{name}";
            while (System.IO.File.Exists(path))
            {
                string patch = $"{path.Substring(0, path.LastIndexOf("."))}~";
                path = patch + path.Substring(path.LastIndexOf("."));
            }
            return path;
        }
    }
}
