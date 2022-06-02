namespace WebApi
{
    /// <summary>
    /// Контакт
    /// </summary>
    public class Contact : IService
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Пустой конструктор услуги.
        /// </summary>
        public Contact() { }
        /// <summary>
        /// Конструктор услуги.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="header">Название услуги</param>
        /// <param name="description">Описание услуги</param>
        public Contact(int id, string header, string description)
        {
            Id = id;
            Header = header;
            Description = description;
        }
    }
}
