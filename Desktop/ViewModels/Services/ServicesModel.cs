using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Desktop
{
    /// <summary>
    /// Модель услуг
    /// </summary>
    public class ServicesModel : WorkModel
    {
        #region Автосвойства
        /// <summary>
        /// Исходная коллекция услуг.
        /// </summary>
        public List<Service> ServicesRaw { get; set; }
        #endregion

        /// <summary>
        /// Конструктор услуг.
        /// </summary>
        /// <param name="editor">Окно редактора</param>
        /// <param name="type">Тип Api</param>
        public ServicesModel(Window editor, string type)
            : base(editor, type)
        {
            GetDataAndShow();
        }

        #region Методы работы с окном Редактора.
        public override void OpenEditor(IElementData element)
        {
            if (element == null) { Editor.DataContext = new ServiceEditVM(this); }
            else { Editor.DataContext = new ServiceEditVM(this, element as Service); }
            Editor.Show();
        }
        public override void GetDataAndShow()
        {
            ServicesRaw = JsonConvert.DeserializeObject<List<Service>>(CRUD.Read(ApiType));
            Show();
        }
        #endregion

        #region Методы вывода данных на экран.
        public override void Show()
        {
            if (Collection != null)
            {
                Collection.Clear();
                IOrderedEnumerable<IElementData> result;
                if (ByType == "По имени")
                {
                    result = from Service in ServicesRaw
                             orderby Service.Id, Service.Id
                             select Service;
                }
                else { result = ServicesRaw.OrderBy(u => u.Header); }
                IElementData[] ProjArray = result.ToArray();
                if (UpDown == "▼")
                {
                    for (int i = 0; i < ProjArray.Length; i++) { AddToServices(ProjArray[i] as Service); }
                }
                else
                {
                    for (int i = ProjArray.Length - 1; i >= 0; i--) { AddToServices(ProjArray[i] as Service); }
                }
                Cnt = Collection.Count;
            }
        }
        /// <summary>
        /// Добавление элемента в выводимую на экран коллекцию.
        /// </summary>
        /// <param name="service">Сервис</param>
        private void AddToServices(Service service)
        {
            if (CheckAll(service))
            {
                ServiceUnit serv = new ServiceUnit();
                serv.DataContext = new ServiceUnitVM(this, service);
                Collection.Add(serv);
            }
        }
        /// <summary>
        /// Проверка на совпадение текста.
        /// </summary>
        /// <param name="service">Сервис</param>
        /// <returns>Содержит/Не содержит</returns>
        private bool CheckAll(Service service)
        {
            bool Check = false;
            Check = Check || service.Header.ToLower().Contains(Findtext.ToLower());
            Check = Check || service.Description.ToLower().Contains(Findtext.ToLower());
            return Check;
        }
        #endregion
    }
}
