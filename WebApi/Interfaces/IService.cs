/// <summary>
/// Интерфейс сервиса.
/// </summary>
public interface IService
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    int Id { get; set; }
    /// <summary>
    /// Название.
    /// </summary>
    string Header { get; set; }
    /// <summary>
    /// Описание.
    /// </summary>
    string Description { get; set; }
}

