/// <summary>
/// Интерфейс авторизации.
/// </summary>
public interface ILogin
{
    /// <summary>
    /// Логин
    /// </summary>
    string Name { get; set; }
    /// <summary>
    /// Пароль
    /// </summary>
    string Password { get; set; }
}
