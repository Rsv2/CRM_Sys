namespace Site
{
    /// <summary>
    /// Контакт
    /// </summary>
    public class Motto
    {
        public int Id { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// Пустой конструктор контакта
        /// </summary>
        public Motto() { }
        /// <summary>
        /// Конструктор контакта
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="text">URL или данные контакта</param>
        public Motto(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
