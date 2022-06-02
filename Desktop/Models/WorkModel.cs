using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Desktop
{
    /// <summary>
    /// Общая модель для Проектов, Блогов и Услуг.
    /// </summary>
    public abstract class WorkModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Поля
        /// <summary>
        /// Коллекция выводимых на экран эллементов.
        /// </summary>
        private ObservableCollection<IElementUnit> collection;
        /// <summary>
        /// Общее число эллементов
        /// </summary>
        private int cnt;
        /// <summary>
        /// Смена направления поиска
        /// </summary>
        private RelayCommand direction;
        /// <summary>
        /// Направление поиска
        /// </summary>
        private string updown;
        /// <summary>
        /// Поиск по имени/дате нажате кнопки
        /// </summary>
        private RelayCommand searchtype;
        /// <summary>
        /// Поиск по имени/дате
        /// </summary>
        private string bytype;
        /// <summary>
        /// Строка поиска текста
        /// </summary>
        private string? findtext;
        /// <summary>
        /// Добавить новый эллемент
        /// </summary>
        private RelayCommand newelement;
        #endregion

        #region Свойства PropertyChanged.
        /// <summary>
        /// Коллекция выводимых на экран проектов.
        /// </summary>
        public ObservableCollection<IElementUnit> Collection
        {
            get => collection;
            set
            {
                collection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Collection)));
            }
        }
        /// <summary>
        /// Общее число эллементов
        /// </summary>
        public int Cnt
        {
            get => cnt;
            set
            {
                cnt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cnt)));
            }
        }
        /// <summary>
        /// Направление поиска
        /// </summary>
        public string UpDown
        {
            get => updown;
            set
            {
                updown = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UpDown)));
            }
        }
        /// <summary>
        /// Поиск по имени/дате
        /// </summary>
        public string ByType
        {
            get => bytype;
            set
            {
                bytype = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ByType)));
            }
        }
        /// <summary>
        /// Строка поиска текста
        /// </summary>
        public string? Findtext
        {
            get => findtext;
            set
            {
                findtext = value;
                if (value != null) Show();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Findtext)));
            }
        }
        #endregion

        #region Команды
        /// <summary>
        /// Смена направления поиска
        /// </summary>
        public RelayCommand Direction => direction ?? (direction = new RelayCommand(obj => ChangeDirection()));
        /// <summary>
        /// Поиск по имени/дате нажате кнопки
        /// </summary>
        public RelayCommand SearchType => searchtype ?? (searchtype = new RelayCommand(obj => ChangeType()));
        /// <summary>
        /// Добавить новый эллемент
        /// </summary>
        public RelayCommand NewElement => newelement ?? (newelement = new RelayCommand(obj => AddElement()));
        #endregion

        #region Автосвойства
        /// <summary>
        /// Раздел api
        /// </summary>
        public string ApiType { get; set; }
        /// <summary>
        /// Окно редактора.
        /// </summary>
        public Window Editor { get; set; }
        /// <summary>
        /// Окно редактора изображений.
        /// </summary>
        public Window ImgWin { get; set; }
        #endregion

        #region Конструкторы.
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="editor">Окно редактора</param>
        /// <param name="apitype">Раздел api</param>
        public WorkModel(Window editor, string apitype)
        {
            Editor = editor;
            ApiType = apitype;
            Fill();
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="editor">Окно редактора</param>
        /// <param name="imgeditor">Окно выбора изображений</param>
        /// <param name="apitype">Раздел api</param>
        public WorkModel(Window editor, ImagesWin imgeditor, string apitype)
            : this(editor, apitype)
        {
            ImgWin = imgeditor;
            Fill();
        }
        /// <summary>
        /// Общие данные.
        /// </summary>
        private void Fill()
        {
            UpDown = "▲";
            ByType = "По имени";
            if (Findtext == null) Findtext = "";
            Collection = new ObservableCollection<IElementUnit>();
        }
        #endregion

        #region Методы обращения к серверу.
        /// <summary>
        /// Отправка проекта на сервер (общее).
        /// </summary>
        /// <param name="edit">Редактировать/Добавить</param>
        /// <param name="picchanged">Изображение изменено да/нет</param>
        /// <param name="project">Проект</param>
        public void SendElement(bool edit, IElementData element)
        {
            if (edit) CRUD.Update(ApiType, JsonConvert.SerializeObject(element));
            else CRUD.Create(ApiType, JsonConvert.SerializeObject(element));
            GetDataAndShow();
            Editor.Close();
        }
        /// <summary>
        /// Получить данные с сервера и вывести на экран.
        /// </summary>
        public abstract void GetDataAndShow();
        /// <summary>
        /// Удалить проект из коллекции.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public void DeleteDataAndShow(string id)
        {
            CRUD.Delete($"{ApiType}/{id}");
            GetDataAndShow();
        }
        #endregion

        #region Методы работы с окном Редактора.
        /// <summary>
        /// Добавить новый элемент.
        /// </summary>
        public void AddElement()
        {
            OpenEditor(null);
        }
        /// <summary>
        /// Открытие окна редактора.
        /// </summary>
        /// <param name="element">Элемент</param>
        public abstract void OpenEditor(IElementData element);
        #endregion

        #region Методы вывода данных на экран.
        /// <summary>
        /// Поиск по имени/дате нажате кнопки
        /// </summary>
        private void ChangeType()
        {
            if (ByType == "По имени") { ByType = "По Id"; }
            else { ByType = "По имени"; }
            Show();
        }
        /// <summary>
        /// Смена направления поиска нажатие кнопки.
        /// </summary>
        private void ChangeDirection()
        {
            if (UpDown == "▲") UpDown = "▼";
            else UpDown = "▲";
            Show();
        }
        /// <summary>
        /// Вывести проекты на экран.
        /// </summary>
        public abstract void Show();
        #endregion

    }
}
