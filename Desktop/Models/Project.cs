namespace Desktop
{
    /// <summary>
    /// Класс Проекта.
    /// </summary>
    public class Project : Service, IProject
    {
        public string Image { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public Project()
            : base()
        {
            Image = "";
        }
        /// <summary>
        /// Конструктор ввода
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="header">Заголовок</param>
        /// <param name="image">Изображение</param>
        /// <param name="description">Описание</param>
        public Project(int id, string header, string image, string description)
            : base(id, header, description)
        {
            Image = image;
        }
    }
}
