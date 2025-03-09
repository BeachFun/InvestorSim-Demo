using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]

/// <summary>
/// Представляет из себя вложение "в долг кому-то"
/// </summary>
public class Dept : IOfferTarget, ICloneable
{
    // создается когда игрок дает кому-то в долг

    private Dept()
    {

    }
    /// <summary>
    /// Создает объект должника
    /// </summary>
    /// <param name="payRate">Процент дохода | 0.054 - 5.4%</param>
    public Dept(DeptorType deptor, DeptorRateType deptorRate, decimal deposit, double? payRate, string currency, DateTime maturityDate)
    {
        Deptor = deptor;
        DeptorRate = deptorRate;
        Deposit = deposit;
        PayRate = payRate;
        Currency = currency;
        MaturityDate = maturityDate;
    }

    /// <summary>
    /// Дать в долг | Метод сам берет деньги у игрока
    /// </summary>
    public bool Lend()
    {
        if (Managers.Player.Wallet[this.Currency] < this.Deposit)
            return false;

        // Расчет свойств неудачной сдачи в долг
        Random rnd = new Random();
        double chance = (int)this.Deptor * 0.01;
        chance *= (int)this.DeptorRate * 0.01;

        if (chance > rnd.NextDouble()) _isScam = false;
        else _isScam = true;
        if (chance > rnd.NextDouble()) _canProfit = false;
        else _canProfit = true;

        Managers.Player.ChangeWalletMoney(this.Currency, -this.Deposit);

        isLended = true;
        return true;
    }



    public DeptorType Deptor { get; private set; }
    public DeptorRateType DeptorRate { get; private set; }

    /// <summary>
    /// Процент дохода от сдачи в долг | 0.03 = 3%
    /// </summary>
    public double? PayRate { get; private set; }
    /// <summary>
    /// Размер сдачи в долг
    /// </summary>
    public decimal Deposit { get; private set; }
    /// <summary>
    /// Валюта
    /// </summary>
    public string Currency { get; private set; }
    /// <summary>
    /// День погашения
    /// </summary>
    public DateTime MaturityDate { get; private set; }

    public Guid Offer { get; private set; }


    private bool _isScam;
    private bool _canProfit;
    private bool isLended;


    public void SetOffer(Guid offer)
    {
        this.Offer = offer;
    }


    internal void DayChangeTrigger()
    {
        if (isLended) return;

        DateTime currDate = Managers.Game.Date;

        // День погашения
        if (currDate == this.MaturityDate)
        {
            Messenger<Dept>.Broadcast(DeptNotice.MATURITY_DATE, this);

            if (!_isScam)
            {
                Managers.Player.ChangeWalletMoney(this.Currency, this.Deposit);
                Messenger<Dept>.Broadcast(DeptNotice.DEPT_REPAID, this);

                if (!_canProfit && this.PayRate.HasValue)
                {
                    Managers.Player.ChangeWalletMoney(this.Currency, this.Deposit * (decimal)this.PayRate.Value);
                }
            }
            else Messenger<Dept>.Broadcast(DeptNotice.DEPT_NOT_REPAID, this);

            Messenger.RemoveListener(GameNotice.DAY_CHANGE, DayChangeTrigger);
        }

        Messenger.Broadcast(PlayerNotice.DEPT_UPDATED);
    }

    public object Clone()
    {
        if (isLended) return new Dept()
        {
            Currency = this.Currency,
            Offer = this.Offer,
            Deposit = this.Deposit,
            Deptor = this.Deptor,
            DeptorRate = this.DeptorRate,
            MaturityDate = this.MaturityDate,
            PayRate = this.PayRate,
            _canProfit = this._canProfit,
            _isScam = this._isScam,
            isLended = this.isLended
        };
        else return new Dept(this.Deptor, this.DeptorRate, this.Deposit, this.PayRate, this.Currency, this.MaturityDate)
        {
            Offer = this.Offer
        };
    }
}