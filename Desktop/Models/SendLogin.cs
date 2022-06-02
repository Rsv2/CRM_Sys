namespace Desktop
{
    public class SendLogin : ILogin
    {
        public string Name { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Конструктор авторизации.
        /// </summary>
        /// <param name="name">Логин</param>
        /// <param name="password">Пароль</param>
        public SendLogin(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
