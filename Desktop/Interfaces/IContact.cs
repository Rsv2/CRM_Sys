/// <summary>
/// Интерфейс контакта.
/// </summary>
public interface IContact
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    int Id { get; set; }
    /// <summary>
    /// URL или данные контакта
    /// </summary>
    string Url { get; set; }
}