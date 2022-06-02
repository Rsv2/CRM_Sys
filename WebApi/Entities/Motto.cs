namespace WebApi
{
    /// <summary>
    /// Мотиватор.
    /// </summary>
    public class Motto
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Текст мотиватора.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Мотиватор.
        /// </summary>
        public Motto() { }
        /// <summary>
        /// Мотиватор.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="text">Текст мотиватора</param>
        public Motto(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
