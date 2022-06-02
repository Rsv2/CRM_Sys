using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;

namespace Desktop
{
    /// <summary>
    /// Вьюмодель контактов
    /// </summary>
    public class ContactsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Поля
        /// <summary>
        /// + на кнопке иконки.
        /// </summary>
        private string plus;
        /// <summary>
        /// Выбрать иконку для нового контакта.
        /// </summary>
        private RelayCommand setpic;
        /// <summary>
        /// Заменить иконку.
        /// </summary>
        private RelayCommand selectpic;
        /// <summary>
        /// Удалить контакт
        /// </summary>
        private RelayCommand delete;
        /// <summary>
        /// Добавить контакт
        /// </summary>
        private RelayCommand send;
        /// <summary>
        /// Измнить данные
        /// </summary>
        private RelayCommand savechanges;
        /// <summary>
        /// Выделенный контакт
        /// </summary>
        private Contact selected;
        /// <summary>
        /// Контакты
        /// </summary>
        private ObservableCollection<Contact> contacts;
        /// <summary>
        /// Url иконки
        /// </summary>
        private string header;
        /// <summary>
        /// Контакт
        /// </summary>
        private string description;
        /// <summary>
        /// Идентификатор
        /// </summary>
        private int id;
        #endregion

        #region Команды
        /// <summary>
        /// Выбрать иконку для нового контакта.
        /// </summary>
        public RelayCommand SetPic => setpic ?? (setpic = new RelayCommand(obj => SetPicture()));
        /// <summary>
        /// Заменить иконку.
        /// </summary>
        public RelayCommand SelectPic => selectpic ?? (selectpic = new RelayCommand(obj => SelectPicture()));
        /// <summary>
        /// Удалить контакт
        /// </summary>
        public RelayCommand Delete => delete ?? (delete = new RelayCommand(obj => DeleteContact()));
        /// <summary>
        /// Добавить контакт
        /// </summary>
        public RelayCommand Send => send ?? (send = new RelayCommand(obj => Create()));
        /// <summary>
        /// Измнить данные
        /// </summary>
        public RelayCommand SaveChanges => savechanges ?? (savechanges = new RelayCommand(obj => Update()));
        #endregion

        #region Свойства PropertyChanged
        /// <summary>
        /// + на кнопке иконки.
        /// </summary>
        public string Plus
        {
            get => plus;
            set
            {
                plus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Plus)));
            }
        }
        /// <summary>
        /// Выделенный контакт
        /// </summary>
        public Contact Selected
        {
            get => selected;
            set
            {
                selected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
            }
        }
        /// <summary>
        /// Контакты
        /// </summary>
        public ObservableCollection<Contact> Contacts
        {
            get => contacts;
            set
            {
                contacts = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Contacts)));
            }
        }
        /// <summary>
        /// Контакт
        /// </summary>
        public string Description
        {
            get => description;
            set
            {
                description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }
        /// <summary>
        /// Url иконки
        /// </summary>
        public string Header
        {
            get => header;
            set
            {
                header = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Header)));
            }
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id
        {
            get => id;
            set
            {
                id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }
        #endregion

        #region Автосвойства
        /// <summary>
        /// Исходная коллекция контактов.
        /// </summary>
        private List<Contact> RawContacts { get; set; }
        /// <summary>
        /// Раздел api
        /// </summary>
        /// 
        private string ApiType { get; set; }
        /// <summary>
        /// Словарь соответствий Id сервера и десктопа.
        /// </summary>
        private Dictionary<int, int> Ids { get; set; }
        /// <summary>
        /// Url сервера.
        /// </summary>
        private string Host { get; set; }
        #endregion

        /// <summary>
        /// Конструктор Контактов.
        /// </summary>
        /// <param name="apitype">Раздел api</param>
        /// <param name="host">Url сервера.</param>
        public ContactsVM(string apitype, string host)
        {
            Host = host.Substring(0, host.IndexOf("api"));
            Plus = "+";
            Header = "";
            ApiType = apitype;
            Contacts = new ObservableCollection<Contact>();
            Read();
        }

        #region Методы вывода на экран
        /// <summary>
        /// Вывести данные на экран.
        /// </summary>
        private void Show()
        {
            Contacts.Clear();
            Ids = new Dictionary<int, int>();
            for (int i = 0; i < RawContacts.Count; i++)
            {
                Ids.Add(i + 1, RawContacts[i].Id);
                Contacts.Add(RawContacts[i]);
                Contacts[Contacts.Count - 1].Id = i + 1;
                Contacts[Contacts.Count - 1].Header = $"{Host}icons/{Contacts[Contacts.Count - 1].Header}";
            }
        }
        #endregion

        #region Методы CRUD
        /// <summary>
        /// Добавить контакт
        /// </summary>
        private void Create()
        {
            if (Description != "" && Description != null && Header != "" && Header != null)
            {
                CRUD.Create(ApiType, Header, Description);
                Header = "";
                Plus = "+";
                Read();
            }
        }
        /// <summary>
        /// Получить данные и вывести на экран.
        /// </summary>
        public void Read()
        {
            try
            {
                RawContacts = JsonConvert.DeserializeObject<List<Contact>>(CRUD.Read(ApiType));
                Show();
            }
            catch
            {
                Read();
            }
        }
        /// <summary>
        /// Изменить контакт
        /// </summary>
        private void Update()
        {
            Contact cont = new Contact(Ids[Selected.Id], Selected.Header, Selected.Description);
            CRUD.Update($"{ApiType}?id={Ids[Selected.Id]}&description={Selected.Description}");
            Read();
        }
        /// <summary>
        /// Удалить контакт
        /// </summary>
        private void DeleteContact()
        {
            CRUD.Delete($"{ApiType}/{Ids[Selected.Id]}");
            Read();
        }
        /// <summary>
        /// Заменить иконку.
        /// </summary>
        private void SelectPicture()
        {
            OpenFileDialog OFD = new OpenFileDialog()
            {
                Title = "Выберите файл изображения",
                Filter = "(.jpg), (.png)|*.jpg; *.png"
            };
            if (OFD.ShowDialog() == true)
            {
                Selected.Header = "";
                CRUD.UpdateContact($"{ApiType}/file?id={Ids[Selected.Id]}&description={Selected.Description}", OFD.FileName);
                Read();
            }
        }
        /// <summary>
        /// Выбрать иконку для нового контакта.
        /// </summary>
        private void SetPicture()
        {
            OpenFileDialog OFD = new OpenFileDialog()
            {
                Title = "Выберите файл изображения",
                Filter = "(.jpg), (.png)|*.jpg; *.png"
            };
            if (OFD.ShowDialog() == true)
            {
                Plus = "";
                Header = OFD.FileName;
            }
        }
        #endregion
    }
}










