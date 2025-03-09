using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]

/// <summary>
/// Содержит информацию о купонных выплатах
/// </summary>
public class Coupon : ICloneable
{
    // TODO: доработать класс

    private Coupon()
    {

    }
    public Coupon(Bond bonds, CouponInfo info) : this(bonds, info.CouponValue, info.PaymentFrequency)
    {

    }
    public Coupon(Bond bonds, decimal couponValue, PaymentFrequency paymentFrequency)
    {
        this.BondsTicket = bonds.Ticket;
        this.BondNominal = bonds.NominalValue;
        this.BondIssueDate = bonds.IssueDate;
        this.CouponValue = couponValue;
        this.PaymentFrequency = paymentFrequency;


        int totalDays = (bonds.MaturityDate - bonds.IssueDate).Days; // устанавливается в родительском конструкторе
        double stepDays = 3650 / (int)this.PaymentFrequency; // TODO: изменить 3650, не понятно откуда, что и зачем

        // Заполнения списка датами выплат купонов
        _payDates = new List<DateTime>();
        for (double days = 0; days < totalDays; days += stepDays)
        {
            _payDates.Add(bonds.IssueDate.AddDays(days));
        }
    }


    #region Properties

    /// <summary>
    /// Тикет облигации, которую представляет данные купоны
    /// </summary>
    public string BondsTicket { get; private set; }
    internal decimal BondNominal { get; set; } // TODO: временное решение
    internal DateTime BondIssueDate { get; set; } // TODO: временное решение
    /// <summary>
    /// Величина купона
    /// </summary>
    public decimal CouponValue { get; set; }
    /// <summary>
    /// Процентная ставка купонов в год (в годовых)
    /// </summary>
    public decimal CouponRate { get => CouponValue * ((int)PaymentFrequency / 10) / this.BondNominal; }
    /// <summary>
    /// Частота выплат
    /// </summary>
    public PaymentFrequency PaymentFrequency { get; set; }

    #endregion


    private List<DateTime> _payDates;


    /// <summary>
    /// Кол-во выплат осталось
    /// </summary>
    public int GetPayDayRemained()
    {
        if (this.GetNextPayDate() == DateTime.MinValue)
            return 0;

        return this._payDates.Count - this._payDates.IndexOf(this.GetNextPayDate());
    }

    /// <summary>
    /// Дата следующей выплаты купона. Возвращает DateTime.MinValue, если выплат не будет. | Сег день учитывается
    /// </summary>
    public DateTime GetNextPayDate()
    {
        foreach (DateTime date in _payDates)
            if (Managers.Game.Date <= date)
                return date;

        return DateTime.MinValue;
    }

    /// <summary>
    /// Предыдущая дата выплаты купона. Если выплат еще не было, возвщает IssueDate (дату выпуска) | Сег день не учитывается
    /// </summary>
    public DateTime GetLastPayDate()
    {
        for (int i = 0; i < _payDates.Count; i++)
            if (Managers.Game.Date <= _payDates[i])
                if (i != 0)
                    return _payDates[i - 1];
                else
                    return this.BondIssueDate;

        return _payDates[^1];
    }

    /// <summary>
    /// Вычисляет общий накопленный купонный доход (НКД) за текущий период
    /// </summary>
    public decimal CalcCouponProfit()
    {
        return CalcCouponProfit(DateTime.MinValue);
    }

    /// <summary>
    /// Вычисляет накопленный купонный доход (НКД) за текущий период с учетом дня покупки 
    /// </summary>
    /// <param name="buyDate">День покуки</param>
    public decimal CalcCouponProfit(DateTime buyDate)
    {
        DateTime nextPayDate = GetNextPayDate();
        DateTime lastPayDate = GetLastPayDate();

        if (nextPayDate == DateTime.MinValue)
            return 0;

        double totalDays = (nextPayDate - lastPayDate).Days;
        double passedDays = (Managers.Game.Date - lastPayDate).Days;
        double skippedDays = (buyDate - lastPayDate).Days;

        if(skippedDays > 0) 
            return (decimal)((passedDays - skippedDays) / totalDays) * CouponValue;
        else
            return (decimal)(passedDays / totalDays) * CouponValue;
    }


    public object Clone()
    {
        return new Coupon()
        {
            BondIssueDate = this.BondIssueDate,
            BondNominal = this.BondNominal,
            BondsTicket = this.BondsTicket,
            CouponValue = this.CouponValue,
            PaymentFrequency = this.PaymentFrequency,
            _payDates = this._payDates is not null ? this._payDates.ToList() : null,
        };
    }
}