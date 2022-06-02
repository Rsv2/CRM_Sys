namespace WebApi
{
    /// <summary>
    /// Проект.
    /// </summary>
    public class Project : IProject
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public Project() { }
        /// <summary>
        /// Конструктор ввода
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="header">Заголовок</param>
        /// <param name="image">Изображение</param>
        /// <param name="description">Описание</param>
        public Project(int id, string header, string image, string description)
        {
            Id = id;
            Header = header;
            Image = image;
            Description = description;
        }
    }
}
