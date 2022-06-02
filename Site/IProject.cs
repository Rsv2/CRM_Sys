/// <summary>
/// Интерфейс проекта.
/// </summary>
public interface IProject
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    int Id { get; set; }
    /// <summary>
    /// Заголовок
    /// </summary>
    string Header { get; set; }
    /// <summary>
    /// Изображение
    /// </summary>
    string Image { get; set; }
    /// <summary>
    /// Описание
    /// </summary>
    string Description { get; set; }
}
