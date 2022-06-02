using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace Desktop
{
    /// <summary>
    /// Вьюмодель заявок.
    /// </summary>
    public class RequestsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Поля
        /// <summary>
        /// За всё время
        /// </summary>
        private RelayCommand forall;
        /// <summary>
        /// Текст кнопки За всё время
        /// </summary>
        private string info;
        /// <summary>
        /// Всего заявок
        /// </summary>
        private string cnt;
        /// <summary>
        /// Текст поиска
        /// </summary>
        private string findtext;
        /// <summary>
        /// Конечная дата
        /// </summary>
        private DateTime end;
        /// <summary>
        /// Начальная дата
        /// </summary>
        private DateTime start;
        /// <summary>
        /// За месяц
        /// </summary>
        private RelayCommand month;
        /// <summary>
        /// За неделю
        /// </summary>
        private RelayCommand week;
        /// <summary>
        /// Вчера
        /// </summary>
        private RelayCommand tomorrow;
        /// <summary>
        /// Сегодня
        /// </summary>
        private RelayCommand today;
        /// <summary>
        /// Смена статуса
        /// </summary>
        private RelayCommand changedselection;
        /// <summary>
        /// Статус заявки
        /// </summary>
        private int selectedstatus;
        /// <summary>
        /// Получить данные с сервера
        /// </summary>
        private RelayCommand getdatascomm;
        /// <summary>
        /// Выделенная заявка
        /// </summary>
        private RequestMessage selected;
        /// <summary>
        /// Исходная коллекция заявок
        /// </summary>
        private List<RequestMessage> rawrequests;
        /// <summary>
        /// Коллекция заявок
        /// </summary>
        private ObservableCollection<RequestMessage> requests;
        #endregion

        #region Команды
        /// <summary>
        /// За всё время
        /// </summary>
        public RelayCommand ForAll => forall ?? (forall = new RelayCommand(obj => ForAllTimes()));
        /// <summary>
        /// За месяц
        /// </summary>
        public RelayCommand Month => month ?? (month = new RelayCommand(obj => ShowMonth()));
        /// <summary>
        /// За неделю
        /// </summary>
        public RelayCommand Week => week ?? (week = new RelayCommand(obj => ShowWeek()));
        /// <summary>
        /// Вчера
        /// </summary>
        public RelayCommand Tomorrow => tomorrow ?? (tomorrow = new RelayCommand(obj => ShowTomorrow()));
        /// <summary>
        /// Сегодня
        /// </summary>
        public RelayCommand Today => today ?? (today = new RelayCommand(obj => ShowToday()));
        /// <summary>
        /// Смена статуса
        /// </summary>
        public RelayCommand ChangedSelection => changedselection ?? (changedselection = new RelayCommand(obj => UpdateStatus()));
        /// <summary>
        /// Получить данные с сервера
        /// </summary>
        public RelayCommand GetDatasComm => getdatascomm ?? (getdatascomm = new RelayCommand(obj => GetDatas()));
        #endregion

        #region Свойства PropertyChanged
        /// <summary>
        /// Текст кнопки За всё время
        /// </summary>
        public string Info
        {
            get => info;
            set
            {
                info = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Info)));
            }
        }
        /// <summary>
        /// Всего заявок
        /// </summary>
        public string Count
        {
            get => cnt;
            set
            {
                cnt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            }
        }
        /// <summary>
        /// Текст поиска
        /// </summary>
        public string Findtext
        {
            get => findtext;
            set
            {
                findtext = value;
                if (RawRequests != null && Findtext != null) Show();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Findtext)));
            }
        }
        /// <summary>
        /// Конечная дата
        /// </summary>
        public DateTime End
        {
            get => end;
            set
            {
                end = value;
                if (End < Start) End = Start;
                if (Requests != null) Show();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(End)));
            }
        }
        /// <summary>
        /// Начальная дата
        /// </summary>
        public DateTime Start
        {
            get => start;
            set
            {
                start = value;
                if (End < Start) End = Start;
                if (Requests != null) Show();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Start)));
            }
        }
        /// <summary>
        /// Статус заявки
        /// </summary>
        public int SelectedStatus
        {
            get => selectedstatus;
            set
            {
                selectedstatus = value;
                MessageBox.Show($"{SelectedStatus}");
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStatus)));
            }
        }
        /// <summary>
        /// Выделенная заявка
        /// </summary>
        public RequestMessage Selected
        {
            get => selected;
            set
            {
                selected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
            }
        }
        /// <summary>
        /// Исходная коллекция заявок
        /// </summary>
        public List<RequestMessage> RawRequests
        {
            get => rawrequests;
            set
            {
                rawrequests = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RawRequests)));
            }
        }
        /// <summary>
        /// Коллекция заявок
        /// </summary>
        public ObservableCollection<RequestMessage> Requests
        {
            get => requests;
            set
            {
                requests = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Requests)));
            }
        }
        #endregion

        #region Автосвойства
        /// <summary>
        /// Раздел api
        /// </summary>
        private string ApiType { get; set; }
        /// <summary>
        /// Дата самой первой заявки.
        /// </summary>
        private DateTime FirstDate { get; set; }
        #endregion

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="apitype">Раздел api</param>
        public RequestsVM(string apitype)
        {
            ApiType = apitype;
            Findtext = "";
            FirstDate = DateTime.Today;
            Start = DateTime.Today;
            End = DateTime.Today;
            GetDatas();
        }

        #region Методы
        /// <summary>
        /// Получить данные с сервера
        /// </summary>
        public void GetDatas()
        {
            RawRequests = JsonConvert.DeserializeObject<List<RequestMessage>>(CRUD.Read(ApiType));
            if (RawRequests.Count > 0)
            {
                FirstDate = RawRequests[0].Date;
            }
            Show();
        }
        /// <summary>
        /// Вывести данные на экран.
        /// </summary>
        private void Show()
        {
            Requests = new ObservableCollection<RequestMessage>();
            for (int i = 0; i < RawRequests.Count; i++)
            {
                if (RawRequests[i].Date >= Start && RawRequests[i].Date < End.AddDays(1))
                {
                    if (CheckAll(RawRequests[i])) Requests.Add(RawRequests[i]);
                }
            }
            Info = $"За всё время: {RawRequests.Count}";
            Count = $"Отсортировано: {Requests.Count}";
        }
        /// <summary>
        /// Смена статуса
        /// </summary>
        private void UpdateStatus()
        {
            CRUD.Update(ApiType, JsonConvert.SerializeObject(Selected));
            GetDatas();
        }
        /// <summary>
        /// Сегодня
        /// </summary>
        private void ShowToday()
        {
            Start = DateTime.Today;
            End = DateTime.Today;
            Show();
        }
        /// <summary>
        /// Вчера
        /// </summary>
        private void ShowTomorrow()
        {
            Start = DateTime.Today.AddDays(-1);
            End = DateTime.Today.AddDays(-1);
            Show();
        }
        /// <summary>
        /// За неделю
        /// </summary>
        private void ShowWeek()
        {
            Start = GetFirstDayOfWeek(DateTime.Today);
            End = DateTime.Today;
            Show();
        }
        /// <summary>
        /// За месяц
        /// </summary>
        private void ShowMonth()
        {
            Start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            End = DateTime.Today;
            Show();
        }
        /// <summary>
        /// Проверка совпадений в тексте.
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Совпало/Не совпало</returns>
        private bool CheckAll(RequestMessage request)
        {
            bool Check = false;
            Check = Check || request.Name.ToLower().Contains(Findtext.ToLower());
            Check = Check || request.Email.ToLower().Contains(Findtext.ToLower());
            Check = Check || request.Message.ToLower().Contains(Findtext.ToLower());
            return Check;
        }
        /// <summary>
        /// Returns the first day of the week that the specified
        /// date is in using the current culture. 
        /// </summary>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDayOfWeek(dayInWeek, defaultCultureInfo);
        }
        /// <summary>
        /// Returns the first day of the week that the specified date 
        /// is in. 
        /// </summary>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            return firstDayInWeek;
        }
        /// <summary>
        /// За всё время
        /// </summary>
        private void ForAllTimes()
        {
            Start = FirstDate;
            End = DateTime.Today;
        }
        #endregion
    }
}