using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// Предлложение вложения средств
/// </summary>
public class Offer
{
    // TODO: !Перенести некоторые свойства в Investment
    internal Guid id;

    private Offer()
    {

    }
    public Offer(string offerType, string description, IOfferTarget offerTarget, string currency, int days)
    {
        OfferType = offerType;
        Description = description;
        OfferTarget = offerTarget;
        Currency = currency;
        Period = new TimeSpan(days, 0, 0, 0);
        IsNew = true;

        id = Guid.NewGuid();
    }
    public Offer(string offerType, string description, IOfferTarget offerTarget, string currency, int days, List<decimal> depositList) 
        : this(offerType, description, offerTarget, currency, days)
    {
        CanFlexibleDeposit = false;
        DepositList = depositList;
    }
    public Offer(string offerType, string description, IOfferTarget offerTarget, string currency, int days, decimal min, decimal? max, decimal step) 
        : this(offerType, description, offerTarget, currency, days)
    {
        CanFlexibleDeposit = true;
        Min = min;
        Max = max;
        Step = step;
    }
    internal Offer(Dictionary<string, object> data)
    {
        IsNew               = (bool) data["IsNew"];
        CanFlexibleDeposit  = (bool) data["CanFlexibleDeposit"];
        Description         = (string) data["Description"];
        OfferType           = (string) data["OfferType"];
        Currency            = (string) data["Currency"];
        Step                = (decimal) data["Step"];
        Min                 = (decimal) data["Min"];
        Max                 = (decimal?) data["Max"];
        Period              = (TimeSpan) data["Period"];
        DepositList         = (List<decimal>) data["DepositList"];
        id                  = (Guid) data["id"];

        OfferTarget = data["OfferTarget"] switch
        {
            Investment => data["OfferTarget"] as Investment,
            Dept => data["OfferTarget"] as Dept,
            _ => data["OfferTarget"] as IOfferTarget
        };
    }


    /// <summary>
    /// Свежое ли предложение?
    /// </summary>
    public bool IsNew { get; private set; }
    /// <summary>
    /// Тип предложения, он же заголовок
    /// </summary>
    public string OfferType { get; private set; }
    /// <summary>
    /// Описание вложения
    /// </summary>
    public string Description { get; private set; }
    /// <summary>
    /// Объект предложения
    /// </summary>
    public IOfferTarget OfferTarget { get; private set; } // TODO: 4 << Serialization
    /// <summary>
    /// Валюта предложения
    /// </summary>
    public string Currency { get; private set; }
    /// <summary>
    /// Период действия предложения
    /// </summary>
    public TimeSpan Period { get; private set; }


    /// <summary>
    /// Можно ли указывать свой размер вложения?
    /// </summary>
    public bool CanFlexibleDeposit { get; private set; }
    /// <summary>
    /// Список вложений. Что-то вроде прайс листа
    /// </summary>
    public List<decimal> DepositList { get; private set; }
    /// <summary>
    /// Минимальный размер депозита
    /// </summary>
    public decimal Min { get; private set; }
    /// <summary>
    /// Максимальный размер депозита
    /// </summary>
    public decimal? Max { get; private set; }
    /// <summary>
    /// Шаг цены
    /// </summary>
    public decimal Step { get; private set; }


    /// <summary>
    /// Делает предложение недействительным
    /// </summary>
    public void Accept()
    {
        Period = TimeSpan.Zero;
    }


    internal void DayChangeTrigger()
    {
        Period = Period.Add(new TimeSpan(-1, 0, 0, 0));
        IsNew = false;

        if (Period.Days == 0)
        {
            Period = TimeSpan.Zero;
            Messenger.Broadcast(OfferControllerNotice.OFFER_NO_RELEVANT);
        }
    }


    public Dictionary<string, object> GetDataCopy()
    {
        Dictionary<string, object> data = new();
        data.Add("CanFlexibleDeposit", this.CanFlexibleDeposit);
        data.Add("Description", this.Description);
        data.Add("OfferType", this.OfferType);
        data.Add("Currency", this.Currency);
        data.Add("Period", this.Period);
        data.Add("IsNew", this.IsNew);
        data.Add("Step", this.Step);
        data.Add("Min", this.Min);
        data.Add("Max", this.Max);
        data.Add("id", this.id);
        data.Add("OfferTarget", this.OfferTarget switch
        {
            Investment => this.OfferTarget as Investment,
            Dept => this.OfferTarget as Dept,
            _ => this.OfferTarget
        });
        if (DepositList is not null) 
             data.Add("DepositList", this.DepositList.ToList());
        else data.Add("DepositList", null);

        return data;
    }

    public object Clone()
    {
        return new Offer()
        {
            CanFlexibleDeposit = this.CanFlexibleDeposit,
            Currency = this.Currency,
            OfferType = this.OfferType,
            Description = this.Description,
            IsNew = this.IsNew,
            Period = this.Period,
            Max = this.Max,
            Min = this.Min,
            Step = this.Step,
            DepositList = this.DepositList.ToList(),
            OfferTarget = this.OfferTarget,
            id = this.id
        };
    }

}