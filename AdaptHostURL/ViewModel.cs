using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace AdaptHostURL
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        #region Поля
        /// <summary>
        /// Адрес хоста Сайта
        /// </summary>
        private string cursitehost;
        /// <summary>
        /// Принять изменения
        /// </summary>
        private RelayCommand confirm;
        /// <summary>
        /// Строка подключения БД сайта
        /// </summary>
        private string sitedb;
        /// <summary>
        /// Строка подключения БД авторизации
        /// </summary>
        private string authdb;
        /// <summary>
        /// Адрес хоста WebApi
        /// </summary>
        private string curhost;
        #endregion

        #region Команды
        /// <summary>
        /// Принять изменения
        /// </summary>
        public RelayCommand Confirm => confirm ?? (confirm = new RelayCommand(obj => ConfirmChanges()));
        #endregion

        #region Свойства PropertyChanged
        /// <summary>
        /// Адрес хоста Сайта
        /// </summary>
        public string CurSiteHost
        {
            get => cursitehost;
            set
            {
                cursitehost = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurSiteHost)));
            }
        }
        /// <summary>
        /// Строка подключения БД сайта
        /// </summary>
        public string SiteDB
        {
            get => sitedb;
            set
            {
                sitedb = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SiteDB)));
            }
        }
        /// <summary>
        /// Строка подключения БД авторизации
        /// </summary>
        public string AuthDB
        {
            get => authdb;
            set
            {
                authdb = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AuthDB)));
            }
        }
        /// <summary>
        /// Адрес хоста WebApi
        /// </summary>
        public string CurHost
        {
            get => curhost;
            set
            {
                curhost = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurHost)));
            }
        }
        #endregion

        #region Авто-Свойства
        /// <summary>
        /// Директория проекта.
        /// </summary>
        private string MainDir { get; set; }
        /// <summary>
        /// Адрес сайта текущий
        /// </summary>
        private string OldSiteHost { get; set; }
        /// <summary>
        /// Адрес WebApi текущий.
        /// </summary>
        private string OldHost { get; set; }
        /// <summary>
        /// Строка БД авторизации текущая.
        /// </summary>
        private string OldAuthDB { get; set; }
        /// <summary>
        /// Строка БД сайта текущая.
        /// </summary>
        private string OldSiteDB { get; set; }
        /// <summary>
        /// Файл appsettings.json
        /// </summary>
        private string DBfile { get; set; }
        /// <summary>
        /// Текст файла appsettings.json
        /// </summary>
        private string Appsettings { get; set; }
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public ViewModel()
        {
            MainDir = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.LastIndexOf("AdaptHostURL"));
            OldHost = GetHost($"{MainDir}WebApi\\WebApi\\Properties\\launchSettings.json");
            CurHost = OldHost;
            OldSiteHost = GetHost($"{MainDir}Site\\Site\\Properties\\launchSettings.json");
            CurSiteHost = OldSiteHost;
            DBfile = $"{MainDir}WebApi\\WebApi\\appsettings.json";
            Appsettings = File.ReadAllText(DBfile);
            OldAuthDB = Appsettings.Substring(Appsettings.IndexOf("Server"));
            OldAuthDB = OldAuthDB.Substring(0, OldAuthDB.IndexOf("\""));
            AuthDB = OldAuthDB;
            OldSiteDB = Appsettings.Substring(Appsettings.LastIndexOf("Server"));
            OldSiteDB = OldSiteDB.Substring(0, OldSiteDB.IndexOf("\""));
            SiteDB = OldSiteDB;
        }

        /// <summary>
        /// Принять изменения
        /// </summary>
        private void ConfirmChanges()
        {
            if (OldAuthDB != AuthDB || OldSiteDB != SiteDB)
            {
                if (AuthDB != SiteDB)
                {
                    Appsettings = Appsettings.Replace(OldAuthDB, AuthDB);
                    Appsettings = Appsettings.Replace(OldSiteDB, SiteDB);
                    File.WriteAllText(DBfile, Appsettings);
                }
            }
            if (OldHost != CurHost || OldSiteHost != CurSiteHost)
            {
                UpdateHost(MainDir);
            }
            OldHost = CurHost;
            OldSiteHost = CurSiteHost;
            OldAuthDB = AuthDB;
            OldSiteDB = SiteDB;
            MessageBox.Show("Адреса сайта, webApi и строки подключения баз данных обновлены.");
        }
        /// <summary>
        /// Обновление url хоста в файлах проекта.
        /// </summary>
        private void UpdateHost(string folder)
        {
            string[] dirs = Directory.GetDirectories(folder);
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < dirs.Length; i++)
            {
                UpdateHost(dirs[i]);
            }
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].ToLower().EndsWith(".cs") || files[i].ToLower().EndsWith(".json"))
                {
                    string temp = File.ReadAllText(files[i]);
                    temp = temp.Replace(OldHost, CurHost);
                    temp = temp.Replace(OldSiteHost, CurSiteHost);
                    File.WriteAllText(files[i], temp);
                }
            }
        }
        /// <summary>
        /// Получить url
        /// </summary>
        /// <param name="lsfile">файл launchSettings.json</param>
        /// <returns></returns>
        private string GetHost(string lsfile)
        {
            string launchSettings = File.ReadAllText(lsfile);
            string output = launchSettings.Substring(launchSettings.LastIndexOf("https:"));
            return output.Substring(0, output.IndexOf(";"));
        }
    }
}





