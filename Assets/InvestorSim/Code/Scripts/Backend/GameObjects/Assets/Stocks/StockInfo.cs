using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Нужен для передачи данных в class Stock. Объекты класса содержат информацию о ценных бумагах. Кол-во, цену и тд
/// </summary>
public struct StockInfo
{
    /// <summary>
    /// Тикет ценной бумаги (наименование)
    /// </summary>
    public string Ticket { get; set; }
    /// <summary>
    /// Описание ценной бумаги
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Рыночная цена ценной бумаги. Поле актуально, для бумаг в свободном обращении.
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Валюта по которой проводятся расчеты
    /// </summary>
    public string Currency { get; set; }
    /// <summary>
    /// Общее кол-во эмиссии. Актуально для бумаг в свободном обращении.
    /// </summary>
    public int Issue { get; set; }
    /// <summary>
    /// Дата выпуска
    /// </summary>
    public DateTime IssueDate { get; set; }
    /// <summary>
    /// Доступное кол-во
    /// </summary>
    public int Amount { get; set; }
    /// <summary>
    /// Биржа/Биржи на которой торгуется ценная бумага
    /// </summary>
    public List<string> StockExchange { get; set; }
}