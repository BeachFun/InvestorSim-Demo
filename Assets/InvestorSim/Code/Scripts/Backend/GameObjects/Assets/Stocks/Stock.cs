using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]

/// <summary>
/// Абстрактный объект ценной бумага
/// </summary>
public abstract class Stock : IAsset
{
    public Stock()
    {
		//Impulses = new List<Impulse>();
	}
	public Stock(string ticket, decimal price, string currency, long issue, long amount, DateTime issueDate, List<string> stockExchanges) : this()
	{
		Ticket = ticket;
		FairPrice = price;
		Currency = currency;
		Issue = issue;
		Amount = amount;
		IssueDate = issueDate;
		StockExchange = stockExchanges;
		Price = price;
	}


	/// <summary>
	/// Тикет ценной бумаги (наименование)
	/// </summary>
	public string Ticket { get; set; }
	/// <summary>
	/// Описание ценной бумаги
	/// </summary>
	public string Description { get; set; }
	/// <summary>
	/// Текущая рыночная цена 
	/// </summary>
	public decimal Price { get; set; }
	/// <summary>
	/// Цена, около которой крутиться рыночная цена
	/// </summary>
	public decimal FairPrice { get; set; } // TODO: перенести в эмитента, чтобы цена двигалась вокруг стоимости эмитента
	/// <summary>
	/// Валюта по которой проводятся расчеты
	/// </summary>
	public string Currency { get; set; }
	/// <summary>
	/// Общее кол-во эмиссии.
	/// </summary>
	public long Issue { get; set; }
	/// <summary>
	/// Доступное кол-во на бирже
	/// </summary>
	public long Amount { get; set; }
	/// <summary>
	/// Дата выпуска
	/// </summary>
	public DateTime IssueDate { get; set; }
	/// <summary>
	/// Биржа/Биржи на которой торгуется ценная бумага
	/// </summary>
	public List<string> StockExchange { get; set; }
	///// <summary>
	///// Действующие импульсы, влияющие на цену
	///// </summary>
	//public List<Impulse> Impulses { get; private set; }


    /// <summary>
    /// Изменяет рыночную стоимость ценной бумаги
    /// </summary>
    /// <param name="delta">Величина изменения цены</param>
    public void ChangePrice(decimal delta)
	{
		if (this.Price + delta < 0)
			this.Price = 0;
		else
			this.Price += delta;
	}
	/// <summary>
	/// Изменяет реальную стоимость ценной бумаги
	/// </summary>
	/// <param name="delta">Величина изменения цены</param>
	public void ChangeFairPrice(decimal delta)
	{
		if (this.FairPrice + delta < 0)
			this.FairPrice = 0;
		else
			this.FairPrice += delta;
	}

  //  /// <summary>
  //  /// Добавляет действующий импульс
  //  /// </summary>
  //  public void AddImpulse(Impulse impulse)
  //  {
		//Impulses.Add(impulse);
  //  }






	/// <summary>
	/// Выводит информацию о выпуске акций в консоль
	/// </summary>
	public void PrintInfo()
	{
		// TODO: Подумать над удалением метода

		string info = string.Format("name: {0}\nprice: {1} {2}\nissue: {3}\nDesc: {4}", Ticket, Price, Currency, Amount, Description);
		Debug.Log(info);
	}
	public void Dispose()
	{
		// TODO: подумать над методом
	}
}