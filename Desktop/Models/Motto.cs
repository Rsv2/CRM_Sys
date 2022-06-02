namespace Desktop
{
    public class Motto
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }
        public string Text { get; set; }

        public Motto() { }
        public Motto(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
