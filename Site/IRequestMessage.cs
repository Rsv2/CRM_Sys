/// <summary>
/// Интерфейс заявки.
/// </summary>
public interface IRequestMessage
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    int Id { get; set; }
    /// <summary>
    /// Имя отправителя.
    /// </summary>
    string Name { get; set; }
    /// <summary>
    /// e-mail отправителя.
    /// </summary>
    string Email { get; set; }
    /// <summary>
    /// Сообщение отправителя.
    /// </summary>
    string Message { get; set; }
    /// <summary>
    /// Время отправления заявки.
    /// </summary>
    DateTime Date { get; set; }
}

