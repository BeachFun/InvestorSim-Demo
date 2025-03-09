using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]

/// <summary>
/// Модель биржы содержащей сведения о состоянии биржи
/// </summary>
public class StockExchange : IGameModel
{
    /// <summary>
    /// Создает пустую модель биржи
    /// </summary>
    public StockExchange()
    {
        _stocks = new();
        _stocks.Add(StockType.Shares, new List<string>());
        _stocks.Add(StockType.Bonds, new List<string>());
    }
    /// <summary>
    /// Создает модель биржи
    /// </summary>
    /// <param name="name">Название биржи</param>
    /// <param name="country">Страна</param>
    /// <param name="currency">Валюта/Валюты</param>
    /// <param name="stocks">Список торгуемых ценных бумаг</param>
    public StockExchange(string name, Country country, string currency/*, List<Stock> stocks*/)
    {
        _stocks = new();
        _stocks.Add(StockType.Shares, new List<string>());
        _stocks.Add(StockType.Bonds, new List<string>());

        Name = name;
        Country = country;
        Currency = currency;

        //_stocks.AddRange(stocks);
    }


    /// <summary>
    /// Название биржи
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Страна в которой находится биржа
    /// </summary>
    public Country Country { get; set; }
    /// <summary>
    /// Валюта/Валюты с которой работет
    /// </summary>
    public string Currency { get; set; } // TODO: подумать над изменением или удалением
    /// <summary>
    /// Список торгуемых ценных бумаг
    /// </summary>
    public Dictionary<StockType, List<string>> Stocks
    {
        get => _stocks;
    }


    // string - stock ticket
    private Dictionary<StockType, List<string>> _stocks;


    public object Clone()
    {
        return new StockExchange()
        {
            Name = this.Name,
            Country = this.Country,
            Currency = this.Currency,
            _stocks = this._stocks.ToDictionary(pair => pair.Key, pair => pair.Value.Select(x => x.Clone() as string).ToList())
        };
    }
}
