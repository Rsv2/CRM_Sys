using System.ComponentModel;
using System.Windows;

namespace Desktop
{
    /// <summary>
    /// Вьюмодель редактора изображений.
    /// </summary>
    public class PicVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Поля
        /// <summary>
        /// Выбрать изображение
        /// </summary>
        private RelayCommand selectcomm;
        /// <summary>
        /// Видимость кнопки выбора
        /// </summary>
        private Visibility selectvis;
        /// <summary>
        /// Удалить изображение с сервера
        /// </summary>
        private RelayCommand delete;
        #endregion

        #region Команды
        /// <summary>
        /// Выбрать изображение
        /// </summary>
        public RelayCommand SelectComm => selectcomm ?? (selectcomm = new RelayCommand(obj => Select()));
        /// <summary>
        /// Удалить изображение с сервера
        /// </summary>
        public RelayCommand Delete => delete ?? (delete = new RelayCommand(obj => DeleteMethod()));
        #endregion

        #region Свойства
        /// <summary>
        /// Видимость кнопки выбора
        /// </summary>
        public Visibility SelectVis
        {
            get => selectvis;
            set
            {
                selectvis = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectVis)));
            }
        }
        /// <summary>
        /// Вьюмодель изображений
        /// </summary>
        public ImagesVM Model { get; set; }
        /// <summary>
        /// Имя изображения
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Url изображения
        /// </summary>
        public string Url { get; set; }
        #endregion

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="model">Вьюмодель изображений</param>
        /// <param name="url">Url изображения</param>
        public PicVM(ImagesVM model, string url)
        {
            Url = url;
            Name = url.Substring(url.LastIndexOf("/") + 1);
            Model = model;
            SelectVis = Model.SelVis;
        }

        #region Методы
        /// <summary>
        /// Удалить изображение с сервера
        /// </summary>
        private void DeleteMethod()
        {
            Model.Delete(Url);
        }
        /// <summary>
        /// Выбрать изображение для проекта.
        /// </summary>
        private void Select()
        {
            Model.SelectPic(Url);
        }
        #endregion
    }
}