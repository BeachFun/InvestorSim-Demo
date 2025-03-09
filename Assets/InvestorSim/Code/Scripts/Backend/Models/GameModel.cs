using System;
using System.Globalization;

[Serializable]

/// <summary>
/// Данные GameManager
/// </summary>
public class GameModel : IGameModel
{
    private GameModel()
    {

    }
    /// <summary>
    /// Создает объект с главными данными игры
    /// </summary>
    /// <param name="date">Дата начала игры</param>
    public GameModel(DateTime date) : this()
    {
        Date = date;
        DaysInGame = 0;
    }

    /// <summary>
    /// Текущая дата в игре
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Кол-во дней с начала игры
    /// </summary>
    public int DaysInGame { get; set; }

    /// <summary>
    /// Увеличивает дату на один день
    /// </summary>
    public void AddDay()
    {
        Date = new GregorianCalendar().AddDays(Date, 1);
        DaysInGame += 1;

        Messenger.Broadcast(GameNotice.DAY_CHANGE);
    }

    public object Clone()
    {
        return new GameModel()
        {
            Date = this.Date,
            DaysInGame = this.DaysInGame
        };
    }
}