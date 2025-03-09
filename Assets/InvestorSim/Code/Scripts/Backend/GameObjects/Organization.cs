using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]

/// <summary>
/// Абстрактная модель организации, представляющая из себя эмитента
/// </summary>
public abstract class Organization : IIssuer
{
    /* Если Capital < 0, то эмитент прикрывает свою организацию
     * 
     * Мысли о расширении:
     * - добавить поля доходы и расходы
     */

    /// <summary>
    /// Создает пустую организацию без метаданных
    /// </summary>
    public Organization()
    {
        Bonds = new List<string>();
        CredibilityRate = 100;
    }
    /// <summary>
    /// Создает организацию с указаннами метаданными
    /// </summary>
    /// <param name="name">Наименование</param>
    /// <param name="description">Описание</param>
    public Organization(string name, string description) : this()
    {
        Name = name;
        Description = description;
    }
    /// <summary>
    /// Создает организацию с указаннами метаданными
    /// </summary>
    /// <param name="name">Наименование</param>
    /// <param name="description">Описание</param>
    /// <param name="type">Тип эмитента</param>
    /// <param name="country">Страна</param>
    public Organization(string name, string description, IssuerType type, Country country) : this()
    {
        Name = name;
        Description = description;
        IssuerType = type;
        Country = country;
    }


    /// <summary>
    /// Наименование организации
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Описание организации
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Дата основания компании
    /// </summary>
    public DateTime FoundationDate { get; set; }

    /// <summary>
    /// Активы организации
    /// </summary>
    public List<string> Assets { get; set; } // TODO: особо и не нужно в проекте
    /// <summary>
    /// Степень доверия к организации (0..100 %)
    /// </summary>
    public float CredibilityRate { get; set; }

    /// <summary>
    /// Общее кол-во денег у организации
    /// </summary>
    private int Capital { get; set; } // TODO: особо и не нужно в проекте
    /// <summary>
    /// Чистая прибыль организации
    /// </summary>
    private int NetProfit { get; set; }// TODO: особо и не нужно в проекте

    /// <summary>
    /// Вид организации эмитента
    /// </summary>
    private IssuerType IssuerType { get; set; }
    /// <summary>
    /// Страна эмитента организации
    /// </summary>
    public Country Country { get; set; }
    /// <summary>
    /// Область деятельности эмитента
    /// </summary>
    public string ActivityArea { get; set; } // last type ActivityArea

    /// <summary>
    /// Список выпусков облигаций организации | string - наименование облигации (тикет)
    /// </summary>
    public List<string> Bonds { get; set; }


    /// <summary>
    /// Выпуск купонных облигаций. Создает выпуск облигаций по следующим метаданным.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ticket">Тикет для акции</param>
    /// <param name="nominalValue">Цена акции</param>
    /// <param name="currency">Валюта акции</param>
    /// <param name="issue">Кол-во акций</param>
    /// <param name="stockExchange">Биржа</param>
    /// <returns>Выпущенные облигации</returns>
    public Bond IssueCouponBonds(string name, string ticket, 
                           decimal nominalValue, decimal price, string currency, 
                           int issue, int amount,
                           decimal couponValue, PaymentFrequency paymentFrequency,
                           DateTime issueDate, DateTime maturityDate,
                           string stockExchange)
    {
        var couponInfo = new CouponInfo()
        {
            CouponValue = couponValue,
            PaymentFrequency = paymentFrequency
        };

        Bond bonds = new Bond(name, ticket, nominalValue, price, currency, issue, amount, issueDate, maturityDate, couponInfo, new List<string>() { stockExchange }, this);
        Bonds.Add(bonds.Ticket);
        return bonds;
    }

    public Bond IssueDiscountBonds(string name, string ticket, 
                                decimal nominalValue, decimal price, string currency, 
                                int issue, int amount, 
                                DateTime issueDate, DateTime maturityDate, 
                                List<string> stockExchange)
    {
        Bond bonds = new(name, ticket, nominalValue, price, currency, issue, amount, issueDate, maturityDate, null, stockExchange, this);
        Bonds.Add(bonds.Ticket);
        return bonds;
    }

    /// <summary>
    /// Выпуск облигаций. Создает выпуск облигаций этого эмитента
    /// </summary>
    /// <param name="stockInfo">Информация об облигации как о ценной бумаге</param>
    /// <param name="bondInfo">Информация об облигации</param>
    /// <param name="couponInfo">Информация о купонах</param>
    /// <returns>Выпущенные облигации</returns>
    public Bond IssueBonds(StockInfo stockInfo, BondInfo bondInfo, CouponInfo couponInfo)
    {
        bondInfo.Issuer = this;

        Bond bonds = new Bond(stockInfo, bondInfo, couponInfo);
        Bonds.Add(bonds.Ticket);
        return bonds;
    }

}