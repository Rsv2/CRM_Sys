namespace WebApi
{
    /// <summary>
    /// Услуга.
    /// </summary>
    public class Service : IService
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Пустой конструктор услуги.
        /// </summary>
        public Service() { }
        /// <summary>
        /// Конструктор услуги.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Название услуги</param>
        /// <param name="description">Описание услуги</param>
        public Service(int id, string name, string description)
        {
            Id = id;
            Header = name;
            Description = description;
        }
    }
}
