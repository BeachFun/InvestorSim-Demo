using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Нужен для передачи данных в class Stock. Объекты класса содержат информацию о ценных бумагах. Кол-во, цену и тд
/// </summary>
public struct BondInfo
{
    /// <summary>
    /// Наименование выпуска облигаций
    /// </summary>
    public string Name
    {
        get;
        set;
    }
    /// <summary>
    /// Номинал. Номинальная стоимость
    /// </summary>
    public decimal NominalValue
    {
        get;
        set;
    }
    /// <summary>
    /// Дата выпуска
    /// </summary>
    public DateTime IssueDate
    {
        get;
        set;
    }
    /// <summary>
    /// Дата погашения
    /// </summary>
    public DateTime MaturityDate
    {
        get;
        set;
    }
    /// <summary>
    /// Ссылка на эмитента, выпустившего облигации
    /// </summary>
    public Organization Issuer
    {
        get;
        set;
    }
}