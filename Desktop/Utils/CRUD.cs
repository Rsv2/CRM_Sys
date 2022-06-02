using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;

namespace Desktop
{
    /// <summary>
    /// Класс методов обращения к серверу.
    /// </summary>
    public static class CRUD
    {
        /// <summary>
        /// Главное окно с паролем.
        /// </summary>
        private static MainWindow Main { get; set; }
        /// <summary>
        /// Инициализация данных класса.
        /// </summary>
        /// <param name="main">Главное окно с паролем.</param>
        public static void Init(MainWindow main)
        {
            Main = main;
        }
        /// <summary>
        /// Добавить данные на сервер (строки) (Create)
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="json">json объект</param>
        public static void Create(string url, string json)
        {
            Main.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.Auth());
            Answer(Main.httpClient.PostAsync($"{Main.Host}{url}", new StringContent(json, Encoding.UTF8, "application/json")).Result.ToString());
        }
        /// <summary>
        /// Добавить данные на сервер (файлы) (Create)
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="data">Массив путей к файлам</param>
        public static void Create(string url, string[] data)
        {
            Main.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.Auth());
            Main.httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
            var multipartContent = new MultipartFormDataContent();
            for (int i = 0; i < data.Length; i++)
            {
                var file1 = new ByteArrayContent(PicsResizer.GetDatas(data[i], 1280.00, 720.00));
                multipartContent.Add(file1, "file", Path.GetFileName(data[i]));
            }
            Answer(Main.httpClient.PostAsync($"{Main.Host}{url}", multipartContent).Result.ToString());
        }
        /// <summary>
        /// Добавить данные на сервер (файл) (Create)
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="pic">Иконка</param>
        /// <param name="description">Контакт</param>
        public static void Create(string url, string pic, string description)
        {
            Main.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.Auth());
            Main.httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
            var multipartContent = new MultipartFormDataContent();
            var file1 = new ByteArrayContent(PicsResizer.GetDatas(pic, 256.00, 256.00));
            multipartContent.Add(file1, "file", Path.GetFileName(pic));
            multipartContent.Add(new StringContent(description), "url");
            Answer(Main.httpClient.PostAsync($"{Main.Host}{url}/file", multipartContent).Result.ToString());
        }
        /// <summary>
        /// Получить данные с сервера (Read)
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>json строка</returns>
        public static string Read(string url)
        {
            Main.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.Auth());
            return Main.httpClient.GetStringAsync($"{Main.Host}{url}").Result;
        }
        /// <summary>
        /// Обновить данные на сервере (Update)
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="json">json объект</param>
        public static void Update(string url, string json)
        {
            Main.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.Auth());
            Answer(Main.httpClient.PutAsync($"{Main.Host}{url}", new StringContent(json, Encoding.UTF8, "application/json")).Result.ToString());
        }
        /// <summary>
        /// Обновить данные на сервере (Update)
        /// </summary>
        /// <param name="url">Url</param>
        public static void Update(string url)
        {
            Main.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.Auth());
            Answer(Main.httpClient.PutAsync($"{Main.Host}{url}", new StringContent("")).Result.ToString());
        }
        /// <summary>
        /// Обновить данные на сервере (Update)
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="pic">путь к иконке</param>
        public static void UpdateContact(string url, string pic)
        {
            Main.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.Auth());
            Main.httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
            var multipartContent = new MultipartFormDataContent();
            var file1 = new ByteArrayContent(PicsResizer.GetDatas(pic, 256.00, 256.00));
            multipartContent.Add(file1, "file", Path.GetFileName(pic));
            Answer(Main.httpClient.PutAsync($"{Main.Host}{url}", multipartContent).Result.ToString());
        }
        /// <summary>
        /// Удалить данные с сервера.
        /// </summary>
        /// <param name="url">Url удаления</param>
        public static void Delete(string url)
        {
            Main.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.Auth());
            Answer(Main.httpClient.DeleteAsync($"{Main.Host}{url}").Result.ToString());
        }
        /// <summary>
        /// Сообщение сервера в случае неудачи.
        /// </summary>
        /// <param name="answer">Ответ сервера</param>
        private static void Answer(string answer)
        {
            if (!answer.Contains("200")) MessageBox.Show(answer);
        }
    }
}
