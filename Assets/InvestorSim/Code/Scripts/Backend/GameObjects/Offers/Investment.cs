using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]

/// <summary>
/// Представляет из себя вложение фальшивое или реальное
/// </summary>
public class Investment : IOfferTarget, ICloneable
{
    // создается когда игрок вкладывается

    private Investment()
    {
        SkippedPays = 0;
    }
    /// <summary>
    /// Объект с данными о вкладе
    /// </summary>
    /// <param name="profitability">Доходность в процентах | 0.03 = 3%</param>
    /// <param name="period">Сррок вложения</param>
    public Investment(decimal profitability, PaymentFrequency payment, string currency, TimeSpan period) : this()
    {
        PayRate = profitability;
        Payment = payment;
        Currency = currency;
        Period = period;
    }


    /// <summary>
    /// Начало дня вложения
    /// </summary>
    /// <param name="deposit">Вклад игрока</param>
    /// <param name="releaseDate">Дата вклада</param>
    public void Start(decimal deposit, DateTime releaseDate)
    {
        Profit = 0;
        Deposit = deposit;
        ReleaseDate = releaseDate;
        MaturityDate = ReleaseDate.Add(this.Period);


        // Расчет дней выплат
        double stepDays = 3650 / (int)Payment; // TODO: изменить 3650, не понятно откуда, что и зачем
        _payDates = new List<DateTime>();
        for (double days = stepDays; days < this.Period.Days; days += stepDays)
        {
            _payDates.Add(releaseDate.AddDays(days));
        }

        // Расчет свойств неудачного вклада
        Random rnd = new Random();
        _isScam = rnd.Next(1, 21) != 1 ? true : false;
        _canOutput = rnd.Next(1, 3) == 1 ? true : false;
        if (_isScam)
            _payCount = rnd.Next(0, _payDates.Count / 2);
        else
            _payCount = _payDates.Count;

        isStarted = true;
    }



    /// <summary>
    /// Тип вклада (предложения
    /// </summary>
    public string InvestmentType { get; set; }
    /// <summary>
    /// Процент выплаты | 0.03 = 3%
    /// </summary>
    public decimal PayRate { get; private set; }
    /// <summary>
    /// Периодичность выплат
    /// </summary>
    public PaymentFrequency Payment { get; private set; }
    /// <summary>
    /// Валюта
    /// </summary>
    public string Currency { get; private set; }
    /// <summary>
    /// Срок вложения
    /// </summary>
    public TimeSpan Period { get; private set; }

    /// <summary>
    /// Размер вклада
    /// </summary>
    public decimal Deposit { get; private set; }
    /// <summary>
    /// День вклада
    /// </summary>
    public DateTime ReleaseDate { get; private set; }
    /// <summary>
    /// День погашения
    /// </summary>
    public DateTime MaturityDate { get; private set; }
    /// <summary>
    /// Доход от вклада
    /// </summary>
    public decimal Profit { get; private set; }

    /// <summary>
    /// Кол-во пропущенных выплат
    /// </summary>
    public int SkippedPays { get; private set; }
    public Guid Offer { get; private set; }


    private bool isStarted; // вложены ли средства
    private bool _isScam; // будет ли выплачен депозит игроку
    // TODO: переделать поле _canOutput
    public bool _canOutput; // будет ли начисляться доход игроку сразу
    private List<DateTime> _payDates; // даты выплат
    private int _payCount; // кол-во выплат игроку (для аферы)



    public void SetOffer(Guid offer)
    {
        this.Offer = offer;
    }

    /// <summary>
    /// Размер выплат
    /// </summary>
    public decimal CalcPayValue() 
    { 
        return Deposit * PayRate; 
    }


    /// <summary>
    /// Кол-во выплат осталось
    /// </summary>
    public int PayDayRemained()
    {
        if (this.NextPayDate() == DateTime.MinValue)
            return 0;

        return this._payDates.Count - this._payDates.IndexOf(this.NextPayDate());
    }

    /// <summary>
    /// Дата следующей начисления дохода. Возвращает DateTime.MinValue, если выплат не будет. | Сег день учитывается
    /// </summary>
    private DateTime NextPayDate()
    {
        foreach (DateTime date in _payDates)
            if (Managers.Game.Date <= date)
                return date;

        return DateTime.MinValue;
    }

    /// <summary>
    /// Предыдущая дата начисления дохода. Если выплат еще не было, возвщает IssueDate (дату выпуска) | Сег день не учитывается
    /// </summary>
    private DateTime LastPayDate()
    {
        for (int i = 0; i < _payDates.Count; i++)
            if (Managers.Game.Date <= _payDates[i])
                if (i != 0)
                    return _payDates[i - 1];
                else
                    return this.ReleaseDate;

        return _payDates[^1];
    }



    internal void DayChangeTrigger()
    {
        if (Deposit == 0) return;

        DateTime currDate = Managers.Game.Date;

        // День выплаты
        if(currDate == this.NextPayDate())
        {
            if (_payCount != 0)
            {
                Messenger<Investment>.Broadcast(InvestmentNotice.PAY_DATE, this);
                this.Profit += this.CalcPayValue();
                _payCount--;

                if (_canOutput)
                {
                    Managers.Player.ChangeWalletMoney(this.Currency, this.CalcPayValue());
                    Messenger<Investment>.Broadcast(InvestmentNotice.PAYOUT_ACCESS, this);
                }
            }
            else
            {
                SkippedPays++;
                Messenger<Investment>.Broadcast(InvestmentNotice.PAYOUT_FAILED, this);
            }
        }

        // День погашения
        if(currDate == this.MaturityDate)
        {
            Messenger<Investment>.Broadcast(InvestmentNotice.MATURITY_DATE, this);

            if (!_isScam)
            {
                Managers.Player.ChangeWalletMoney(this.Currency, this.Deposit);
                Messenger<Investment>.Broadcast(InvestmentNotice.PAYOUT_NOMINAL_ACCESS, this);

                if (!_canOutput)
                {
                    Managers.Player.ChangeWalletMoney(this.Currency, this.Profit);
                    Messenger<Investment>.Broadcast(InvestmentNotice.PAYOUT_ACCESS, this);
                }
            }
            else
                Messenger<Investment>.Broadcast(InvestmentNotice.PAYOUT_NOMINAL_FAILED, this);

            Messenger.RemoveListener(GameNotice.DAY_CHANGE, DayChangeTrigger);
        }

        Messenger.Broadcast(PlayerNotice.INVESTMENT_UPDATED);
    }


    public object Clone()
    {
        if (isStarted) return new Investment()
        {
            Offer = this.Offer,
            PayRate = this.PayRate,
            Payment = this.Payment,
            Currency = this.Currency,
            Period = this.Period,
            SkippedPays = this.SkippedPays,
            Profit = this.Profit,
            Deposit = this.Deposit,
            ReleaseDate = this.ReleaseDate,
            MaturityDate = this.MaturityDate,
            InvestmentType = this.InvestmentType,
            _canOutput = this._canOutput,
            _isScam = this._isScam,
            _payCount = this._payCount,
            _payDates = this._payDates.ToList(),
            isStarted = this.isStarted
        };
        else return new Investment(this.PayRate, this.Payment, this.Currency, this.Period)
        {
            Offer = this.Offer,
            InvestmentType = this.InvestmentType
        };
    }
}
