using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Net.Http.Headers;
using System.Text;

namespace Site
{
    /// <summary>
    /// Класс методов обращения к серверу.
    /// </summary>
    public static class CRUD
    {
        public static HttpClient? httpClient = new HttpClient();
        public static string? Host = "https://localhost:7133/api/";

        public static string? Token { get; set; }

        /// <summary>
        /// Добавить данные на сервер (строки) (Create)
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="json">json объект</param>
        public static void Create(string url, string json)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            Answer(httpClient.PostAsync($"{Host}{url}", new StringContent(json, Encoding.UTF8, "application/json")).Result.ToString());
        }
        /// <summary>
        /// Добавить данные на сервер (файлы) (Create)
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="data">Массив путей к файлам</param>
        public static void Create(string url, IFormFileCollection pics)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
            var multipartContent = new MultipartFormDataContent();
            for (int i = 0; i < pics.Count; i++)
            {
                var file1 = new ByteArrayContent(ResizePic.Resize(pics[i], 1280.00, 720.00));
                multipartContent.Add(file1, "file", pics[i].FileName);
            }
            Answer(httpClient.PostAsync($"{Host}{url}", multipartContent).Result.ToString());
        }

        public static void CreateContact(string url, IFormFile pic, string description)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
            var multipartContent = new MultipartFormDataContent();
            var file1 = new ByteArrayContent(ResizePic.Resize(pic, 256.00, 256.00));
            multipartContent.Add(file1, "file", pic.FileName);
            multipartContent.Add(new StringContent(description), "url");
            Answer(httpClient.PostAsync($"{Host}{url}", multipartContent).Result.ToString());
        }

        public static void UpdateContact(string url, IFormFile pic)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
            var multipartContent = new MultipartFormDataContent();
            var file1 = new ByteArrayContent(ResizePic.Resize(pic, 256.00, 256.00));
            multipartContent.Add(file1, "file", pic.FileName);
            Answer(httpClient.PutAsync($"{Host}{url}", multipartContent).Result.ToString());
        }
        public static void UpdateContact(string url)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
            Answer(httpClient.PutAsync($"{Host}{url}", new StringContent("")).Result.ToString());
        }

        /// <summary>
        /// Получить данные с сервера (Read)
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>json строка</returns>
        public static string Read(string url)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            return httpClient.GetStringAsync($"{Host}{url}").Result;
        }
        /// <summary>
        /// Обновить данные на сервере (Update)
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="json">json объект</param>
        public static void Update(string url, string json)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            Answer(httpClient.PutAsync($"{Host}{url}", new StringContent(json, Encoding.UTF8, "application/json")).Result.ToString());
        }
        /// <summary>
        /// Удалить данные с сервера.
        /// </summary>
        /// <param name="url">Url удаления</param>
        public static void Delete(string url)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            Answer(httpClient.DeleteAsync($"{Host}{url}").Result.ToString());
        }
        /// <summary>
        /// Сообщение сервера в случае неудачи.
        /// </summary>
        /// <param name="answer">Ответ сервера</param>
        private static void Answer(string answer)
        {
            if (!answer.Contains("200")) Console.WriteLine(answer);
        }
    }
}
