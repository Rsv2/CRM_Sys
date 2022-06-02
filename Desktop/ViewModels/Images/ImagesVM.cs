using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Desktop
{
    /// <summary>
    /// Вьюмодель изображений.
    /// </summary>
    public class ImagesVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Поля
        /// <summary>
        /// Добавить изображение
        /// </summary>
        private RelayCommand addpiccomm;
        /// <summary>
        /// Выбранное изображение из коллекции
        /// </summary>
        private PicUnit selectedpic;
        /// <summary>
        /// Коллекция изображений на сервере
        /// </summary>
        private ObservableCollection<PicUnit> pics;
        /// <summary>
        /// Общее число картинок
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
        /// Строка поиска текста
        /// </summary>
        private string findtext;
        #endregion

        #region Команды
        /// <summary>
        /// Добавить изображение
        /// </summary>
        public RelayCommand AddPicComm => addpiccomm ?? (addpiccomm = new RelayCommand(obj => Create()));
        /// <summary>
        /// Смена направления поиска
        /// </summary>
        public RelayCommand Direction => direction ?? (direction = new RelayCommand(obj => ChangeDirection()));
        #endregion

        #region Свойства PropertyChanged
        /// <summary>
        /// Выбранное изображение из коллекции
        /// </summary>
        public PicUnit SelectedPic
        {
            get => selectedpic;
            set
            {
                selectedpic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPic)));
            }
        }
        /// <summary>
        /// Коллекция изображений на сервере
        /// </summary>
        public ObservableCollection<PicUnit> Pics
        {
            get => pics;
            set
            {
                pics = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pics)));
            }
        }
        /// <summary>
        /// Общее число картинок
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
        /// Строка поиска текста
        /// </summary>
        public string Findtext
        {
            get => findtext;
            set
            {
                findtext = value;
                if (Findtext != null) Show();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Findtext)));
            }
        }
        #endregion

        #region Автосвойства
        /// <summary>
        /// Список url картинок на сервере.
        /// </summary>
        public List<string> PicsRaw { get; set; }
        /// <summary>
        /// Режим Окно/UI.
        /// </summary>
        public Visibility SelVis { get; set; }
        /// <summary>
        /// Раздел Api
        /// </summary>
        private string ApiType { get; set; }
        /// <summary>
        /// Вьюмодель окна редактора проектов.
        /// </summary>
        private ProjectEditVM ProjVM { get; set; }
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="apitype">Раздел Api</param>
        /// <param name="selvis">Видимость кнопки выбора</param>
        public ImagesVM(string apitype, Visibility selvis)
        {
            SelVis = selvis;
            ApiType = apitype;
            Fill();
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="apitype">Раздел Api</param>
        /// <param name="selvis">Видимость кнопки выбора</param>
        /// <param name="projvm">Вьюмодель редактора проектов</param>
        public ImagesVM(string apitype, Visibility selvis, ProjectEditVM projvm)
            : this(apitype, selvis)
        {
            ProjVM = projvm;
            Fill();
        }
        /// <summary>
        /// Общие данные конструкторов.
        /// </summary>
        private void Fill()
        {
            UpDown = "▲";
            Findtext = "";
            Pics = new ObservableCollection<PicUnit>();
            Read();
        }
        #endregion

        #region Методы CRUD.
        /// <summary>
        /// Добавить изображение (Create)
        /// </summary>
        private void Create()
        {
            OpenFileDialog OFD = new OpenFileDialog()
            {
                Title = "Выберите файлы изображений",
                Filter = "(.jpg), (.png)|*.jpg; *.png",
                Multiselect = true
            };
            if (OFD.ShowDialog() == true)
            {
                CRUD.Create($"{ApiType}/file", OFD.FileNames);
                Read();
            }
        }
        /// <summary>
        /// Получить данные с сервера (Read)
        /// </summary>
        public void Read()
        {
            PicsRaw = JsonConvert.DeserializeObject<List<string>>(CRUD.Read(ApiType));
            Show();
        }
        /// <summary>
        /// Удалить изображение по имени (Delete)
        /// </summary>
        /// <param name="name">Имя изображения</param>
        public void Delete(string name)
        {
            List<Project> proj = JsonConvert.DeserializeObject<List<Project>>(CRUD.Read("Project"));
            List<Project> blog = JsonConvert.DeserializeObject<List<Project>>(CRUD.Read("Blog"));
            if (proj.Find(u => u.Image == name) != null) MessageBox.Show("Изображение используется в Проектах.");
            else if (blog.Find(u => u.Image == name) != null) MessageBox.Show("Изображение используется в Блогах.");
            else
            {
                CRUD.Delete($"{ApiType}/{name.Substring(name.LastIndexOf("/") + 1)}");
                Read();
            }
        }
        #endregion

        #region Методы вывода данных на экран.
        /// <summary>
        /// Вывод данных на экран.
        /// </summary>
        private void Show()
        {
            if (Pics != null)
            {
                Pics.Clear();
                PicsRaw.Sort();
                if (UpDown == "▼") for (int i = 0; i < PicsRaw.Count; i++) AddToPics(PicsRaw[i]);
                else for (int i = PicsRaw.Count - 1; i >= 0; i--) AddToPics(PicsRaw[i]);
                Cnt = Pics.Count;
            }
        }
        /// <summary>
        /// Смена направления поиска
        /// </summary>
        private void ChangeDirection()
        {
            if (UpDown == "▲") UpDown = "▼";
            else UpDown = "▲";
            Show();
        }
        /// <summary>
        /// Добавить PicUnit в коллекцию Pics.
        /// </summary>
        /// <param name="picurl">url изображения</param>
        private void AddToPics(string picurl)
        {
            if (picurl.Substring(picurl.LastIndexOf("/") + 1).ToLower().Contains(Findtext.ToLower()))
            {
                PicUnit pic = new PicUnit();
                pic.DataContext = new PicVM(this, picurl);
                Pics.Add(pic);
            }
        }
        /// <summary>
        /// Выбор изображения.
        /// </summary>
        /// <param name="url">Url изображения</param>
        public void SelectPic(string url)
        {
            ProjVM.Image = url;
            ProjVM.Model.ImgWin.Close();
        }
        #endregion
    }
}