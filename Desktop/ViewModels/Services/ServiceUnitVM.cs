namespace Desktop
{
    /// <summary>
    /// Вьюмодель UI отдельной услуги
    /// </summary>
    public class ServiceUnitVM
    {
        #region Поля
        /// <summary>
        /// Удалить
        /// </summary>
        private RelayCommand delete;
        /// <summary>
        /// Редактировать
        /// </summary>
        private RelayCommand edit;
        #endregion

        #region Команды
        /// <summary>
        /// Удалить
        /// </summary>
        public RelayCommand Delete => delete ?? (delete = new RelayCommand(obj => DeleteMetod()));
        /// <summary>
        /// Редактировать
        /// </summary>
        public RelayCommand Edit => edit ?? (edit = new RelayCommand(obj => EditMetod()));
        #endregion

        #region Автосвойства
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Описнаие
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// Текущая услуга
        /// </summary>
        public Service CurService { get; set; }
        /// <summary>
        /// Модель услуг
        /// </summary>
        private ServicesModel Model { get; set; }
        #endregion

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="model">Модель сервисов</param>
        /// <param name="service">Сервис</param>
        public ServiceUnitVM(ServicesModel model, Service service)
        {
            Model = model;
            CurService = service;
            Id = service.Id;
            Header = service.Header;
            Description = service.Description;
        }

        #region Методы
        /// <summary>
        /// Редактировать
        /// </summary>
        private void EditMetod()
        {
            Model.OpenEditor(CurService);
        }
        /// <summary>
        /// Удалить
        /// </summary>
        private void DeleteMetod()
        {
            Model.DeleteDataAndShow($"{Id}");
        }
        #endregion
    }
}
