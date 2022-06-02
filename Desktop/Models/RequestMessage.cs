using System;

namespace Desktop
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
    }
}
