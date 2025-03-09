using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]

/// <summary>
/// Представляет из себя модель данных компании
/// </summary>
public class Corporation : Organization, IGameObject, ICloneable
{
    // TODO: В классе не учтены политика дивидендов корпорации, т е не реализован мехнанизм политики дивидендов


    public Corporation()
    {

    }
    public Corporation(CompanyType type) : this()
    {
        Type = type;
    }


    /// <summary>
    /// Тип корпорации
    /// </summary>
    public CompanyType Type { get; private set; }
    /// <summary>
    /// Акции обыкновенные
    /// </summary>
    public string SharesOrdTicket { get; private set; }
    /// <summary>
    /// Акции привилегированные
    /// </summary>
    public string SharesPrivTicket { get; private set; }



    /// <summary>
    /// Объеявление о выплате дивидендов на собрании акционеров
    /// </summary>
    /// <param name="type">Тип акции корпорации</param>
    /// <param name="dividendValue">Размер дивидендов</param>
    /// <returns>Вычисленный дивиденд</returns>
    public Dividend AnnounceDividend(ShareType type, decimal dividendValue)
    {
        DateTime currentDate = Managers.Game.Date;
        DateTime CutOffDate = this.Country switch
        {
            // TODO: Настроить даты див отсечек
            Country.Russia => Managers.Game.Date.AddMonths(1),
            Country.USA => Managers.Game.Date.AddMonths(1),
            Country.Norway => Managers.Game.Date.AddMonths(1),
            Country.Italy => Managers.Game.Date.AddMonths(1),
            _ => DateTime.MinValue
        };

        Dividend dividend = new Dividend()
        {
            DeclarationDate = currentDate,
            CutOffDate = CutOffDate,
            PayDate = CutOffDate.AddDays(new System.Random().Next(0, 5)),
            DividendValue = dividendValue
        };

        if (type == ShareType.Ordinary)
        {
            IAsset asset = Controllers.Assets.FindAsset(this.SharesOrdTicket);
            if (asset is not null)
            {
                Shares shares = asset as Shares;
                shares.AddDividend(dividend);
                Messenger<Shares>.Broadcast(CorporationNotice.DIVIDEND_ANNOUNCED, shares);
            }
        }
        if (type == ShareType.Privileged)
        {
            IAsset asset = Controllers.Assets.FindAsset(this.SharesPrivTicket);
            if (asset is not null)
            {
                Shares shares = asset as Shares;
                shares.AddDividend(dividend);
                Messenger<Shares>.Broadcast(CorporationNotice.DIVIDEND_ANNOUNCED, shares);
            }
        }

        return dividend;
    }


    /// <summary>
    /// Выпускает акции указанного типа. Корпорация не хранит данные акций, а только ссылку в качестве тиккета
    /// </summary>
    /// <param name="type">Тип акций</param>
    /// <param name="ticket">Тикет для акции</param>
    /// <param name="price">Цена акции</param>
    /// <param name="currency">Валюта акции</param>
    /// <param name="issue">Кол-во акций</param>
    /// <param name="stockExchange">Биржа</param>
    /// <returns>Выпущенные акции</returns>
    public Shares IssueShares(ShareType type, string ticket, decimal price, string currency, int issue, DateTime issueDate, string stockExchange)
    {
        var list = new List<string>();
        list.Add(stockExchange);
        return IssueShares(type, ticket, price, currency, issue, issueDate, list);
    }

    /// <summary>
    /// Выпускает акции указанного типа. Корпорация не хранит данные акций, а только ссылку в качестве тиккета
    /// </summary>
    /// <param name="type">Тип акций</param>
    /// <param name="ticket">Тикет для акции</param>
    /// <param name="price">Цена акции</param>
    /// <param name="currency">Валюта акции</param>
    /// <param name="issue">Кол-во акций</param>
    /// <param name="stockExchanges">Биржа/Биржы</param>
    /// <returns>Выпущенные акции</returns>
    public Shares IssueShares(ShareType type, string ticket, decimal price, string currency, int issue, DateTime issueDate, List<string> stockExchanges)
    {
        if ((type == ShareType.Ordinary && this.SharesOrdTicket != null) || 
            (type == ShareType.Privileged && this.SharesPrivTicket != null))
        {
            Debug.Log(string.Format("У корпорации {0} уже есть акции такого вида", this.Name));
            return null;
        }

        Shares shares = new Shares(type, ticket, price, currency, issue, issue, issueDate, MovingForce.PosStrong, stockExchanges, this); // Выпуск акций с указанием на какой бирже будет проводиться торговля

        if (type == ShareType.Ordinary)
            this.SharesOrdTicket = shares.Ticket;
        if (type == ShareType.Privileged)
            this.SharesPrivTicket = shares.Ticket;

        return shares;
    }


    public object Clone()
    {
        return new Corporation()
        {
            Name = this.Name,
            Description = this.Description,
            Type = this.Type,
            Country = this.Country,
            ActivityArea = this.ActivityArea,
            FoundationDate = this.FoundationDate,
            SharesOrdTicket = this.SharesOrdTicket,
            SharesPrivTicket = this.SharesPrivTicket,
            CredibilityRate = this.CredibilityRate,
            Bonds = this.Bonds.Select(x => x.Clone() as string).ToList(),
            //Assets = this.Assets.Select(x => x.Clone() as string).ToList()
        }; ;
    }
}