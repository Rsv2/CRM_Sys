using System;
using System.ComponentModel;
using System.Windows;

namespace Desktop
{
    public class DesktopVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Поля
        /// <summary>
        /// Вьюмодель мотиваторов.
        /// </summary>
        private MottosVM mottosviewmodel;
        /// <summary>
        /// Вьюмодель заявок
        /// </summary>
        private RequestsVM requestviewmodel;
        /// <summary>
        /// Вьюмодель контактов
        /// </summary>
        private ContactsVM contactsviewmodel;
        /// <summary>
        /// Вьюмодель сервисов
        /// </summary>
        private WorkVM servicesviewmodel;
        /// <summary>
        /// Вьюмодель блогов
        /// </summary>
        private WorkVM blogsviewmodel;
        /// <summary>
        /// Вьюмодель изображений
        /// </summary>
        private ImagesVM picsviewmodel;
        /// <summary>
        /// Viewmodel проектов.
        /// </summary>
        private WorkVM projectsviewmodel;
        /// <summary>
        /// Модель
        /// </summary>
        private DesktopModel model;
        #endregion

        #region Свойства PropertyChanged
        /// <summary>
        /// Вьюмодель мотиваторов.
        /// </summary>
        public MottosVM MottosViewModel
        {
            get => mottosviewmodel;
            set
            {
                mottosviewmodel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MottosViewModel)));
            }
        }
        /// <summary>
        /// Вьюмодель заявок
        /// </summary>
        public RequestsVM RequestViewModel
        {
            get => requestviewmodel;
            set
            {
                requestviewmodel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RequestViewModel)));
            }
        }
        /// <summary>
        /// Вьюмодель контактов
        /// </summary>
        public ContactsVM ContactsViewModel
        {
            get => contactsviewmodel;
            set
            {
                contactsviewmodel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContactsViewModel)));
            }
        }
        /// <summary>
        /// Вьюмодель сервисов
        /// </summary>
        public WorkVM ServicesViewModel
        {
            get => servicesviewmodel;
            set
            {
                servicesviewmodel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServicesViewModel)));
            }
        }
        /// <summary>
        /// Вьюмодель блогов
        /// </summary>
        public WorkVM BlogsViewModel
        {
            get => blogsviewmodel;
            set
            {
                blogsviewmodel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BlogsViewModel)));
            }
        }
        /// <summary>
        /// Вьюмодель изображений
        /// </summary>
        public ImagesVM PicsViewModel
        {
            get => picsviewmodel;
            set
            {
                picsviewmodel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PicsViewModel)));
            }
        }
        /// <summary>
        /// Viewmodel проектов.
        /// </summary>
        public WorkVM ProjectsViewModel
        {
            get => projectsviewmodel;
            set
            {
                projectsviewmodel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProjectsViewModel)));
            }
        }
        /// <summary>
        /// Модель
        /// </summary>
        public DesktopModel Model
        {
            get => model;
            set
            {
                model = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Model)));
            }
        }
        #endregion

        #region Автосвойства
        /// <summary>
        /// Окно добавления/редактирования услуг.
        /// </summary>
        public static ServiceWin? ServWin { get; set; }
        /// <summary>
        /// Окно добавления/редактирования блогов.
        /// </summary>
        public static ProjectWin? BlogWin { get; set; }
        /// <summary>
        /// Окно выбора изображений блогов.
        /// </summary>
        public static ImagesWin? ImgWinBlog { get; set; }
        /// <summary>
        /// Окно добавления/редактирования проектов.
        /// </summary>
        public static ProjectWin? ProjWin { get; set; }
        /// <summary>
        /// Окно выбора изображений проектов.
        /// </summary>
        public static ImagesWin? ImgWinProj { get; set; }
        #endregion

        public DesktopVM(DesktopModel model)
        {
            WinsClosing();
            Model = model;
            BlogWin.Title = "Блог";
            MottosViewModel = new MottosVM("Mottos");
            ProjectsViewModel = new WorkVM(new ProjectsModel(ProjWin, ImgWinProj, "Project"));
            BlogsViewModel = new WorkVM(new ProjectsModel(BlogWin, ImgWinBlog, "Blog"));
            PicsViewModel = new ImagesVM("Pics", Visibility.Collapsed);
            ServicesViewModel = new WorkVM(new ServicesModel(ServWin, "Services"));
            ContactsViewModel = new ContactsVM("Contacts", Model.MainWin.Host);
            RequestViewModel = new RequestsVM("Request");
        }

        #region Закрытие окон.
        private void WinsClosing()
        {
            MainWindow.Desktop.Closed += Desktop_Closed;
            ImgWinProj = new ImagesWin();
            ImgWinProj.Closing += ImgWinProj_Closing;
            ImgWinBlog = new ImagesWin();
            ImgWinBlog.Closing += ImgWinBlog_Closing;
            ProjWin = new ProjectWin();
            ProjWin.Closing += ProjWin_Closing;
            BlogWin = new ProjectWin();
            BlogWin.Closing += BlogWin_Closing;
            ServWin = new ServiceWin();
            ServWin.Closing += ServWin_Closing;
        }
        private void Desktop_Closed(object? sender, EventArgs e)
        {
            if (ProjWin != null) ProjWin.Close();
            if (BlogWin != null) BlogWin.Close();
            if (ServWin != null) ServWin.Close();
            Model.MainWin.Close();
        }
        private void ServWin_Closing(object? sender, CancelEventArgs e)
        {
            if (MainWindow.Desktop.IsVisible)
            {
                e.Cancel = true;
                ServWin.Hide();
            }
        }
        private void BlogWin_Closing(object? sender, CancelEventArgs e)
        {
            if (MainWindow.Desktop.IsVisible)
            {
                e.Cancel = true;
                BlogWin.Hide();
            }
            if (ImgWinBlog != null) ImgWinBlog.Close();
        }
        private void ProjWin_Closing(object? sender, CancelEventArgs e)
        {
            if (MainWindow.Desktop.IsVisible)
            {
                e.Cancel = true;
                ProjWin.Hide();
            }
            if (ImgWinProj != null) ImgWinProj.Close();
        }
        private void ImgWinBlog_Closing(object? sender, CancelEventArgs e)
        {
            if (MainWindow.Desktop.IsVisible)
            {
                e.Cancel = true;
                ImgWinBlog.Hide();
            }
            UpdatePicData();
        }
        private void ImgWinProj_Closing(object? sender, CancelEventArgs e)
        {
            if (MainWindow.Desktop.IsVisible)
            {
                e.Cancel = true;
                ImgWinProj.Hide();
            }
            UpdatePicData();
        }
        /// <summary>
        /// Обновление изображений при закрытии окна.
        /// </summary>
        private void UpdatePicData()
        {
            PicsViewModel = new ImagesVM("Pics", Visibility.Collapsed);
        }
        #endregion
    }
}








