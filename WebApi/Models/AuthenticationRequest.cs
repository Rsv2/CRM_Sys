namespace WebApi
{
    /// <summary>
    /// Запрос на авторизацию.
    /// </summary>
    public class AuthenticationRequest
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
