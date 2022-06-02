using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Окно десктопа.
        /// </summary>
        public static DesktopWin Desktop { get; set; }
        /// <summary>
        /// Http клиент.
        /// </summary>
        public HttpClient httpClient { get; set; }
        /// <summary>
        /// Адрес хоста.
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Вьюмодель основного окна.
        /// </summary>
        public DesktopVM DesktopViewModel { get; set; }

        /// <summary>
        /// Окно авторизации.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            CRUD.Init(this);
            Host = "https://localhost:7133/api/";
            httpClient = new HttpClient();
        }
        /// <summary>
        /// Запуск приложения при авторизации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterDesktop(object sender, RoutedEventArgs e)
        {
            Enter();
        }
        /// <summary>
        /// Запуск приложения при авторизации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterDesktop(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) Enter();
        }
        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <returns>Токен</returns>
        public string Auth()
        {
            SendLogin model = new SendLogin(Login.Text, Password.Password);
            string url = Host + "Authentication";
            return httpClient.PostAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                mediaType: "application/json")).Result.Content.ReadAsStringAsync().Result;
        }
        /// <summary>
        /// Вход в приложение.
        /// </summary>
        private void Enter()
        {
            if (Login.Text != "" && Password.Password != "" && Login.Text != null && Password.Password != null)
            {
                if (Auth() != "Unauthorize")
                {
                    Hide();
                    Desktop = new DesktopWin();
                    DesktopModel dtmodel = new DesktopModel(this);
                    Desktop.DataContext = new DesktopVM(dtmodel);
                    Desktop.Show();
                }
            }
        }
    }
}
