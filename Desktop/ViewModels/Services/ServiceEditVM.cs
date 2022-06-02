using System.ComponentModel;

namespace Desktop
{
    /// <summary>
    /// Вьюмодель UI редактора услуг
    /// </summary>
    public class ServiceEditVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Поля
        /// <summary>
        /// Отправить данные на сервер
        /// </summary>
        private RelayCommand sendcomm;
        /// <summary>
        /// Идентификатор
        /// </summary>
        private int id;
        /// <summary>
        /// Описание услуги
        /// </summary>
        private string description;
        /// <summary>
        /// Название услуги
        /// </summary>
        private string header;
        #endregion

        #region Команды
        /// <summary>
        /// Отправить данные на сервер
        /// </summary>
        public RelayCommand SendComm => sendcomm ?? (sendcomm = new RelayCommand(obj => Send()));
        #endregion

        #region Свойства PropertyChanged
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
        /// <summary>
        /// Описание услуги
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
        /// Название услуги
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
        #endregion

        #region Автосвойства
        /// <summary>
        /// Флаг Редактирования/Добавления.
        /// </summary>
        bool Edit { get; set; }
        /// <summary>
        /// Редактируемая услуга.
        /// </summary>
        private Service CurService { get; set; }
        /// <summary>
        /// Модель услуг.
        /// </summary>
        private ServicesModel Model { get; set; }
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model">Модель услуг</param>
        public ServiceEditVM(ServicesModel model)
        {
            CurService = new Service();
            Model = model;
            Edit = false;
            Fill();
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model">Модель услуг</param>
        /// <param name="service">Редактируемая услуга</param>
        public ServiceEditVM(ServicesModel model, Service service)
            : this(model)
        {
            CurService = service;
            Edit = true;
            Fill();
        }
        /// <summary>
        /// Общие данные конструкторов
        /// </summary>
        private void Fill()
        {
            Id = CurService.Id;
            Header = CurService.Header;
            Description = CurService.Description;
        }
        #endregion

        #region Методы
        /// <summary>
        /// Отправить данные на сервер
        /// </summary>
        private void Send()
        {
            Model.SendElement(Edit, new Service(Id, Header, Description));
        }
        #endregion

    }
}