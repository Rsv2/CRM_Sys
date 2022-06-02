using System.ComponentModel;

namespace Desktop
{
    /// <summary>
    /// Вьюмодель окна редактора проектов.
    /// </summary>
    public class ProjectEditVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Поля
        /// <summary>
        /// Идентификатор.
        /// </summary>
        private int id;
        /// <summary>
        /// Отправить на сервер
        /// </summary>
        private RelayCommand sendform;
        /// <summary>
        /// Выбрать изображение
        /// </summary>
        private RelayCommand selectimage;
        /// <summary>
        /// Изображение
        /// </summary>
        private string image;
        /// <summary>
        /// Описание
        /// </summary>
        private string description;
        /// <summary>
        /// Заголовок
        /// </summary>
        private string header;
        /// <summary>
        /// Проект
        /// </summary>
        private Project curproject;
        #endregion

        #region Команды
        /// <summary>
        /// Отправить на сервер
        /// </summary>
        public RelayCommand SendForm => sendform ?? (sendform = new RelayCommand(obj => Send()));
        /// <summary>
        /// Выбрать изображение
        /// </summary>
        public RelayCommand SelectImage => selectimage ?? (selectimage = new RelayCommand(obj => Select()));
        #endregion

        #region Свойства
        /// <summary>
        /// Идентификатор.
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
        /// Изображение
        /// </summary>
        public string Image
        {
            get => image;
            set
            {
                image = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
            }
        }
        /// <summary>
        /// Описание
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
        /// Заголовок
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
        /// Проект
        /// </summary>
        public Project CurProject
        {
            get => curproject;
            set
            {
                curproject = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurProject)));
            }
        }
        /// <summary>
        /// Флаг редактирования.
        /// </summary>
        private bool Edit { get; set; }
        /// <summary>
        /// Модель проектов.
        /// </summary>
        public ProjectsModel Model { get; set; }
        #endregion

        #region Конструкторы.
        /// <summary>
        /// Конструктор добавления.
        /// </summary>
        /// <param name="model">Модель</param>
        public ProjectEditVM(ProjectsModel model)
        {
            Model = model;
            CurProject = new Project();
            FillForm();
            Edit = false;
        }
        /// <summary>
        /// Конструктор редактирования.
        /// </summary>
        /// <param name="model">Модель</param>
        /// <param name="project">Прокт</param>
        public ProjectEditVM(ProjectsModel model, Project project)
            : this(model)
        {
            CurProject = project;
            FillForm();
            Edit = true;
        }
        /// <summary>
        /// Заполнение формы.
        /// </summary>
        private void FillForm()
        {
            Id = CurProject.Id;
            Image = CurProject.Image;
            Header = CurProject.Header;
            Description = CurProject.Description;
        }
        #endregion

        #region Методы.
        /// <summary>
        /// Выбрать изображение
        /// </summary>
        private void Select()
        {
            Model.OpenImgWin(this);
        }
        /// <summary>
        /// Отправить на сервер
        /// </summary>
        private void Send()
        {
            Model.SendElement(Edit, new Project(Id, Header, Image, Description));
        }
        #endregion
    }
}