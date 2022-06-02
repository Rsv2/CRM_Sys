using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Desktop
{
    public class MottosVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Поля
        /// <summary>
        /// Выбранный мотиватор
        /// </summary>
        private Motto selected;
        /// <summary>
        /// Удалить мотиватор
        /// </summary>
        private RelayCommand delete;
        /// <summary>
        /// Исходная коллекция мотиваторов
        /// </summary>
        private List<Motto> rawmottos;
        /// <summary>
        /// Коллекция мотиваторов
        /// </summary>
        private ObservableCollection<Motto> mottos;
        /// <summary>
        /// Сохранить изменения
        /// </summary>
        private RelayCommand savechanges;
        /// <summary>
        /// Отправить мотиватор на сервер
        /// </summary>
        private RelayCommand send;
        /// <summary>
        /// Текст мотиватора
        /// </summary>
        private string text;
        #endregion

        #region Команды
        /// <summary>
        /// Удалить мотиватор
        /// </summary>
        public RelayCommand Delete => delete ?? (delete = new RelayCommand(obj => DeleteMotto()));
        /// <summary>
        /// Сохранить изменения
        /// </summary>
        public RelayCommand SaveChanges => savechanges ?? (savechanges = new RelayCommand(obj => Update()));
        /// <summary>
        /// Отправить мотиватор на сервер
        /// </summary>
        public RelayCommand Send => send ?? (send = new RelayCommand(obj => Create()));
        #endregion

        #region Свойства
        /// <summary>
        /// Выбранный мотиватор
        /// </summary>
        public Motto Selected
        {
            get => selected;
            set
            {
                selected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
            }
        }
        /// <summary>
        /// Исходная коллекция мотиваторов
        /// </summary>
        public List<Motto> RawMottos
        {
            get => rawmottos;
            set
            {
                rawmottos = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RawMottos)));
            }
        }
        /// <summary>
        /// Коллекция мотиваторов
        /// </summary>
        public ObservableCollection<Motto> Mottos
        {
            get => mottos;
            set
            {
                mottos = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mottos)));
            }
        }
        /// <summary>
        /// Текст мотиватора
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }
        #endregion

        #region Автосвойства
        /// <summary>
        /// Раздел api
        /// </summary>
        /// 
        private string ApiType { get; set; }
        /// <summary>
        /// Словарь соответствий Id сервера и десктопа.
        /// </summary>
        private Dictionary<int, int> Ids { get; set; }
        #endregion

        public MottosVM(string apitype)
        {
            ApiType = apitype;
            Mottos = new ObservableCollection<Motto>();
            Read();
        }

        #region Методы CRUD
        /// <summary>
        /// Добавить мотиватор
        /// </summary>
        private void Create()
        {
            if (Text != "" && Text != null)
            {
                CRUD.Create(ApiType, JsonConvert.SerializeObject(new Motto(0, Text)));
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
                RawMottos = JsonConvert.DeserializeObject<List<Motto>>(CRUD.Read(ApiType));
                Show();
            }
            catch
            {
                Read();
            }
        }
        /// <summary>
        /// Изменить мотиватор
        /// </summary>
        private void Update()
        {
            CRUD.Update(ApiType, JsonConvert.SerializeObject(new Motto(Ids[Selected.Id], Selected.Text)));
            Read();
        }
        /// <summary>
        /// Удалить мотиватор
        /// </summary>
        private void DeleteMotto()
        {
            CRUD.Delete($"{ApiType}/{Ids[Selected.Id]}");
            Read();
        }
        #endregion

        #region Методы вывода на экран
        /// <summary>
        /// Вывести данные на экран.
        /// </summary>
        private void Show()
        {
            Mottos.Clear();
            Ids = new Dictionary<int, int>();
            for (int i = 0; i < RawMottos.Count; i++)
            {
                Ids.Add(i + 1, RawMottos[i].Id);
                Mottos.Add(RawMottos[i]);
                Mottos[Mottos.Count - 1].Id = i + 1;
            }
        }
        #endregion
    }
}