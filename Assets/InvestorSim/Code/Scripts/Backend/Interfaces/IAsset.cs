using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Задает свойства актива.
/// </summary>
public interface IAsset : IDisposable
{
    /// <summary>
    /// Наименование актива (Тикет)
    /// </summary>
    string Ticket { get; set; }
    /// <summary>
    /// Рыночная цена актива
    /// </summary>
    decimal Price { get; set; }
    /// <summary>
    /// Реальная цена актива
    /// </summary>
    decimal FairPrice { get; set; }
    /// <summary>
    /// Кол-во актива
    /// </summary>
    long Amount { get; set; }
    /// <summary>
    /// Валюта/Валюты за которую происходит купля/продажа
    /// </summary>
    string Currency { get; set; }


    /// <summary>
    /// Функция изменения рыночнай цены. Формула: Price = Price + delta
    /// </summary>
    /// <param name="delta">Разница на которую должна измениться цена</param>
    void ChangePrice(decimal delta);

    /// <summary>
    /// Функция изменения реальной цены.  Формула: FairPrice = FairPrice + delta
    /// </summary>
    /// <param name="delta">Разница на которую должна измениться цена</param>
    void ChangeFairPrice(decimal delta);
}