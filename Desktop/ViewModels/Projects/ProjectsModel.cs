using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Desktop
{
    /// <summary>
    /// Модель проектов и блогов
    /// </summary>
    public class ProjectsModel : WorkModel
    {
        #region Автосвойства
        /// <summary>
        /// Исходная коллекция проектов.
        /// </summary>
        public List<Project> ProjectsRaw { get; set; }
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="editor">Окно редактора</param>
        /// <param name="imgeditor">Окно редактора изображений</param>
        /// <param name="apitype">Раздел Api</param>
        public ProjectsModel(Window editor, ImagesWin imgeditor, string apitype)
            : base(editor, imgeditor, apitype)
        {
            GetDataAndShow();
        }

        #region Методы работы с окном Редактора.
        /// <summary>
        /// Открытие окна редактора.
        /// </summary>
        /// <param name="project"></param>
        public override void OpenEditor(IElementData element)
        {
            if (element == null) { Editor.DataContext = new ProjectEditVM(this); }
            else { Editor.DataContext = new ProjectEditVM(this, element as Project); }
            Editor.Show();
        }
        /// <summary>
        /// Открытие окна изображений.
        /// </summary>
        /// <param name="curproj">Вьюмодель текущего редактора</param>
        public void OpenImgWin(ProjectEditVM curproj)
        {
            ImgWin.DataContext = new ImagesVM("Pics", Visibility.Visible, curproj);
            ImgWin.Show();
        }
        #endregion

        #region Методы вывода данных на экран.
        public override void GetDataAndShow()
        {
            Show();
        }
        public override void Show()
        {
            ProjectsRaw = JsonConvert.DeserializeObject<List<Project>>(CRUD.Read(ApiType));
            if (Collection != null)
            {
                Collection.Clear();
                IOrderedEnumerable<IElementData> result;
                if (ByType == "По имени")
                {
                    result = from Project in ProjectsRaw
                             orderby Project.Id, Project.Id
                             select Project;
                }
                else { result = ProjectsRaw.OrderBy(u => u.Header); }
                IElementData[] ProjArray = result.ToArray();
                if (UpDown == "▼")
                {
                    for (int i = 0; i < ProjArray.Length; i++) { AddToProjects(ProjArray[i] as Project); }
                }
                else
                {
                    for (int i = ProjArray.Length - 1; i >= 0; i--) { AddToProjects(ProjArray[i] as Project); }
                }
                Cnt = Collection.Count;
            }
        }
        /// <summary>
        /// Добавление проекта в выводимую на экран коллекцию.
        /// </summary>
        /// <param name="proj">Проект</param>
        private void AddToProjects(Project proj)
        {
            if (CheckAll(proj))
            {
                ProjectUnit project = new ProjectUnit();
                project.DataContext = new ProjectUnitVM(this, proj);
                Collection.Add(project);
            }
        }
        /// <summary>
        /// Проверка на совпадение текста в полях проекта.
        /// </summary>
        /// <param name="project">Проект</param>
        /// <returns>Содержит/Не содержит</returns>
        private bool CheckAll(Project project)
        {
            bool Check = false;
            Check = Check || project.Header.ToLower().Contains(Findtext.ToLower());
            Check = Check || project.Description.ToLower().Contains(Findtext.ToLower());
            return Check;
        }
        #endregion
    }
}