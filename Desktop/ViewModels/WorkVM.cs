using System.ComponentModel;

namespace Desktop
{
    /// <summary>
    /// Общая Вьюмодель для Проектов, Блогов и Услуг.
    /// </summary>
    public class WorkVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Поля
        /// <summary>
        /// Модель
        /// </summary>
        private WorkModel model;
        #endregion

        #region Свойства
        /// <summary>
        /// Модель
        /// </summary>
        public WorkModel Model
        {
            get => model;
            set
            {
                model = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Model)));
            }
        }
        #endregion

        public WorkVM(WorkModel workmodel)
        {
            Model = workmodel;
        }
    }
}





