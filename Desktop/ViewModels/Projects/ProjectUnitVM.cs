namespace Desktop
{
    /// <summary>
    /// Вьюмодель UI отдельного проекта
    /// </summary>
    public class ProjectUnitVM
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

        #region Свойства
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
        /// Изображение
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Модель проекта
        /// </summary>
        public Project CurProject { get; set; }
        /// <summary>
        /// Модель проектов
        /// </summary>
        private ProjectsModel Model { get; set; }
        #endregion

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="model">Модель проектов</param>
        /// <param name="project">Проект</param>
        public ProjectUnitVM(ProjectsModel model, Project project)
        {
            Model = model;
            CurProject = project;
            Id = project.Id;
            Header = project.Header;
            Description = project.Description;
            Image = project.Image;
        }

        #region Методы
        /// <summary>
        /// Редактировать
        /// </summary>
        private void EditMetod()
        {
            Model.OpenEditor(CurProject);
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












