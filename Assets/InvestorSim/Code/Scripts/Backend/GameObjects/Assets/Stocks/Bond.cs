using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]

/// <summary>
/// Модель облигаций выпущенных эмитентом. Представляет из себя выпуск со сведениями об облигации
/// </summary>
public class Bond : Stock, IGameObject, ICloneable
{
    private Bond()
    {

    }
    /// <summary>
    /// Создает выпуск облигаций. Создает объект выпуска облигаций.
    /// </summary>
    /// <param name="name">Наименование</param>
    /// <param name="ticket">Тикет</param>
    /// <param name="price">Стартовая цена</param>
    /// <param name="nominalValue">Номинал</param>
    /// <param name="currency">Валюта</param>
    /// <param name="issue">Эммисия</param>
    /// <param name="amount">Доступное кол-во</param>
    /// <param name="issueDate">Дата выпуска</param>
    /// <param name="maturityDate">Дата погашения</param>
    /// <param name="stockExchanges">Биржа/Биржы</param>
    /// <param name="issuer">Эмитент</param>
    public Bond(string name,
                string ticket,
                decimal nominalValue,
                decimal price,
                string currency,
                int issue,
                int amount,
                DateTime issueDate,
                DateTime maturityDate,
                CouponInfo? couponInfo,
                List<string> stockExchanges,
                Organization issuer)
        : base(ticket, price, currency, issue, amount, issueDate, stockExchanges)
    {
        this.Name = name;
        this.NominalValue = nominalValue;
        this.MaturityDate = maturityDate;
        this.Issuer = issuer.Name;

        if (couponInfo != null)
            this.Coupon = new Coupon(this, couponInfo.Value);
    }

    public Bond(StockInfo stockInfo, BondInfo bondInfo, CouponInfo? couponInfo)
        : this(stockInfo, bondInfo.Name, bondInfo.NominalValue, bondInfo.IssueDate, bondInfo.MaturityDate, couponInfo, bondInfo.Issuer)
    {

    }
    public Bond(StockInfo info, string name, decimal nominalValue, DateTime issueDate, DateTime maturityDate, CouponInfo? couponInfo, Organization issuer) 
        : base(info.Ticket, info.Price, info.Currency, info.Issue, info.Amount, info.IssueDate, info.StockExchange)
    {
        this.Name = name;
        this.NominalValue = nominalValue;
        this.MaturityDate = maturityDate;
        this.IssueDate = issueDate;
        this.Issuer = issuer.Name;

        if (couponInfo != null)
            this.Coupon = new Coupon(this, couponInfo.Value);
    }


    /// <summary>
    /// Наименование выпуска облигаций
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Тип облигаций
    /// </summary>
    public BondType Type
    {
        get
        {
            if (Coupon is null)
                return BondType.Discount;
            else
                return BondType.Coupon;
        }
    }
    /// <summary>
    /// Номинал. Номинальная стоимость
    /// </summary>
    public decimal NominalValue { get; set; }
    /// <summary>
    /// Дата погашения
    /// </summary>
    public DateTime MaturityDate { get; set; }
    /// <summary>
    /// Оставшееся время до даты погашения
    /// </summary>
    public TimeSpan ExpirationSpan
    {
        get
        {
            if(Managers.Game.Date < IssueDate)
                return TimeSpan.Zero;

            return MaturityDate - Managers.Game.Date;
        }
    }
    /// <summary>
    /// Амортизация
    /// </summary>
    public bool IsDepreciation { get; set; }
    /// <summary>
    /// Имя эмитента, выпустившего облигации
    /// </summary>
    public string Issuer { get; set; }


    /// <summary>
    /// Инфа о купонах облигации. Если купонов, нет, то облигация дисконтная
    /// </summary>
    public Coupon Coupon { get; private set; }


    /// <summary>
    /// Доходность к погашению, выраженное в % в год
    /// </summary>
    public decimal CalcProfitPrecentToMaturity()
    {
        if (Coupon is null)
        {
            return ((NominalValue - Price) / Price) * 365 / ExpirationSpan.Days;
        }
        else
        {
            return ((NominalValue - Price + (Coupon.CouponValue * Coupon.GetPayDayRemained() - Coupon.CalcCouponProfit())) / Price) * 365 / ExpirationSpan.Days;
        }
    }


    /// <summary>
    /// Триггер проверки событий
    /// </summary>
    internal void DayChangeTrigger()
    {
        // this.Issuer.CredibilityRate;

        if (Coupon is null)
            return;

        // Проверка дня выплаты купона
        if (Coupon.GetNextPayDate() == Managers.Game.Date)
        {
            Messenger<Bond>.Broadcast(StockExchangeNotice.COUPON_PAYMENT, this);
        }

        // Проверка дня погашения облигации
        if (this.MaturityDate == Managers.Game.Date)
        {
            Messenger<Bond>.Broadcast(StockExchangeNotice.BONDS_MATURITY_DATE, this);
        }
    }


    public object Clone()
    {
        //UnityEngine.Debug.Log($"bond save started {this.Ticket}");
        var data = new Bond()
        {
            Amount = this.Amount,
            Issuer = this.Issuer,
            Currency = this.Currency,
            Description = this.Description,
            FairPrice = this.FairPrice,
            Issue = this.Issue,
            IssueDate = this.IssueDate,
            Price = this.Price,
            Ticket = this.Ticket,
            IsDepreciation = this.IsDepreciation,
            MaturityDate = this.MaturityDate,
            Name = this.Name,
            NominalValue = this.NominalValue,
            StockExchange = this.StockExchange.ToList(),
            Coupon = this.Coupon is not null ? this.Coupon.Clone() as Coupon : null
        };
        //UnityEngine.Debug.Log($"bond save end {this.Ticket}");
        return data;
    }
}