namespace Site
{
    /// <summary>
    /// Класс услуги.
    /// </summary>
    public class Service
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Пустой конструктор услуги.
        /// </summary>
        public Service()
        {
            Id = 0;
            Header = "";
            Description = "";
        }
        /// <summary>
        /// Конструктор услуги.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="header">Название услуги</param>
        /// <param name="description">Описание услуги</param>
        public Service(int id, string header, string description)
        {
            Id = id;
            Header = header;
            Description = description;
        }
    }
}
