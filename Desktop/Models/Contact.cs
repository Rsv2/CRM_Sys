namespace Desktop
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
        /// Пустой конструктор контакта.
        /// </summary>
        public Contact() { }
        /// <summary>
        /// Конструктор услуги.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="header">Название</param>
        /// <param name="description">Описание</param>
        public Contact(int id, string header, string description)
        {
            Id = id;
            Header = header;
            Description = description;
        }
    }
}
