using System;
/// <summary>
/// Интерфейс заявки.
/// </summary>
public interface IRequestMessage
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    int Id { get; set; }
    /// <summary>
    /// Имя отправителя.
    /// </summary>
    string Name { get; set; }
    /// <summary>
    /// e-mail отправителя.
    /// </summary>
    string Email { get; set; }
    /// <summary>
    /// Сообщение отправителя.
    /// </summary>
    string Message { get; set; }
    /// <summary>
    /// Статус заявки
    /// 0 - Получена. Начальный статус. Гость заполнил форму и отправил данные, они поступили в систему, но ещё не были обработаны.
    /// 1 - В работе. Администратор связался с гостем для уточнения деталей.
    /// 2 - Выполнена. Услуга оказана.
    /// 3 - Отклонена. Заявка не подходит или сделана ботом.
    /// 4 - Отменена. Насчёт заявки передумали или она потеряла актуальность.
    /// </summary>
    int Status { get; set; }
    /// <summary>
    /// Время отправления заявки.
    /// </summary>
    DateTime Date { get; set; }
}

