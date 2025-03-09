using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]

/// <summary>
/// Новость связанная с каким-либо активом
/// </summary>
public class News : ICloneable
{
    public News()
    {
        //this.AssetsRelate = new List<IAsset>();
        this.Related = new Dictionary<string, Impulse>();
    }
    public News(string title, string content, DateTime releaseDate, Impulse impulse, bool isPositive, NewsPower power) : this()
    {
        // TODO: доработать конструктор или удалить

        this.Title = title;
        this.Content = content;
        this.ReleaseDate = releaseDate;
        //this.Impulse = impulse;
        this.IsPositive = isPositive;
        this.Power = power;
    }

    /// <summary>
    /// Заголовок новости
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Содержание новости
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    /// Активы на которые влияет новость cо своими импульсами | string - наименование (ticket)
    /// </summary>
    public Dictionary<string, Impulse> Related { get; set; }
    /// <summary>
    /// Дата выхода новости
    /// </summary>
    public DateTime ReleaseDate { get; set; } // last return: int
    /// <summary>
    /// Продолжительность действия новости | Item1 - min, Item2 - max
    /// </summary>
    public (int, int) DurationOfAction { get; set; }
    /// <summary>
    /// Источник новости
    /// </summary>
    public string Source { get; set; }


    #region Скрытые показатели

    /// <summary>
    /// Позитивна ли новость?
    /// </summary>
    public bool IsPositive { get; set; }
    /// <summary>
    /// Сила новости
    /// </summary>
    public NewsPower Power { get; set; }
    ///// <summary>
    ///// Актуальность новости
    ///// </summary>
    //public bool IsActually { get => Impulse != null ? Impulse.IsActive : false; }
    /// <summary>
    /// Актуальность новости
    /// </summary>
    public bool IsActually { get => Related.Values.Any(x => x != null) ? Related.Values.Any(x => x.IsActive) : false; }

    #endregion


    public object Clone()
    {
        return new News()
        {
            Title = this.Title,
            Content = this.Content,
            ReleaseDate = this.ReleaseDate,
            Power = this.Power,
            DurationOfAction = this.DurationOfAction,
            Source = this.Source,
            IsPositive = this.IsPositive,
            Related = this.Related.ToDictionary(pair => pair.Key, pair => pair.Value.Clone() as Impulse)
        };
    }
}
//private bool ChangeModel; // pump - true, pump/dump - false


public enum NewsPower
{
    Unknown = 0,
    Low = 3,
    Medium = 6,
    High = 11
}