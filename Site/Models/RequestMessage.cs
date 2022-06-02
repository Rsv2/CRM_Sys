namespace Site
{
    /// <summary>
    /// Заявка.
    /// </summary>
    public class RequestMessage : IRequestMessage
    {
        public int Id { get; set; }
        public string Name { get;set;}
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public int Status { get; set; }
        /// <summary>
        /// Пустой конструктор заявки.
        /// </summary>
        public RequestMessage() { }
        /// <summary>
        /// Конструктор заявки.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Имя отправителя</param>
        /// <param name="email">e-mail отправителя</param>
        /// <param name="message">Сообщение отправителя</param>
        public RequestMessage(int id, string name, string email, string message)
        {
            Id = id;
            Status = 1;
            Name = name;
            Email = email;
            Message = message;
            Date = DateTime.Now;
        }
    }
}
