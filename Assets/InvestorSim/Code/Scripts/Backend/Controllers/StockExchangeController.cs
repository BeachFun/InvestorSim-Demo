using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляющий фондовыми биржами
/// </summary>
public class StockExchangeController : MonoBehaviour, IGameController, IDataCopable
{
	// ставка комиссии за операцию
	public const decimal comissionRate = (decimal)0.003; // 0.3%
	// ставка налога за прибыль
	public const decimal taxRate = (decimal)0.13; // 13%

	public ControllerStatus status { get; private set; }

	/// <summary>
	/// Списки ценных бумаг торгуемые на бирже
	/// </summary>
	public Dictionary<StockType, List<string>> StockList { get => _model.Stocks; }


	private StockExchange _model;


	#region Методы запуска и Инициализации

	private void OnDestroy()
	{
		if (status == ControllerStatus.Started)
		{
			Messenger.RemoveListener(GameNotice.DAY_CHANGE, DayChangeTrigger);
		}
	}

	public IEnumerator Startup()
	{
		Debug.Log("Stock exchange controller starting...");
		status = ControllerStatus.Initializing;

		yield return null;

		// Если на момент запуска нет данных, то создаются данные по-умолчанию
		if (_model is null) _model = new StockExchange();

		Messenger.AddListener(GameNotice.DAY_CHANGE, DayChangeTrigger);

		status = ControllerStatus.Started;
		Debug.Log("Stock exchange controller started...");
	}

	public void UpdateData(StockExchange data)
	{
		_model = data;
	}

    #endregion


    public decimal CalcComission(Stock stock, int amount)
	{
		return stock.Price * amount * comissionRate;
	}

	public decimal CalcSum(Stock stock, int amount)
    {
		return stock.Price * amount;
	}

	/// <summary>
	/// Вычисляет общий накопленный купонный доход от определенной облигации. Если облигация типа Discount, то вернет null
	/// </summary>
	public decimal? CalcCouponValue(Bond bond, int amount)
    {
		if (bond.Type == BondType.Discount) return null;

		return bond.Coupon.CalcCouponProfit() * amount;
	}

	/// <summary>
	/// Вычисляет накопленный купонный доход игрока от определенной облигации. Если у игрока нет такой облигации? выплата сегодня или выплат не осталось вернет null. 
	/// </summary>
	public decimal? CalcCouponValue(Bond bond)
    {
		if (bond.Type == BondType.Discount) return null;

		AssetPurchase asset = Managers.Player.FindAsset(AssetType.Bonds, bond);

		if (asset is null || bond.Coupon.GetNextPayDate() == Managers.Game.Date) return null;

		decimal NKD = 0;
		for (int i = 0; i < asset.Buys.Count; i++)
		{
			NKD += bond.Coupon.CalcCouponProfit(asset.Buys[i].Date) * asset.Buys[i].Amount;
		}

		return NKD;
	}

	/// <summary>
	/// Вычисляет размер налога при продаже определенной ценной бумаги
	/// </summary>
	/// <param name="stock"></param>
	/// <param name="amount"></param>
	/// <returns></returns>
	public decimal CalcTax(Stock stock, int amount)
    {
		// TODO: Временное решение, может работать некорректно. Переделать вычисления комиссии

		AssetPurchase asset = Managers.Player.FindAsset(stock);

		if (asset is null)
        {
			Debug.Log("Такого актива нет у игрока");
			return -1;
        }
		if (asset.Amount <= 0)
		{
			Debug.Log("Такого актива нет у игрока");
			return -2;
		}

		decimal buyPrice = asset.BuyPrices.Average(x => x);
		decimal price = stock.Price * (1 + comissionRate);

		if (buyPrice > price)
		{
			Debug.Log($"Нет дохода\nсредняя цена покупки - {buyPrice}\nрыночная цена - {price}");
			return 0;
		}

		return (price - buyPrice) * amount * taxRate;
	}


	/// <summary>
	/// Покупка ценной бумаги
	/// </summary>
	/// <param name="stock">Ценная бумага</param>
	/// <param name="amount">Кол-во</param>
	/// <exception cref="Exception">Либо ценная бумага пустая, либо принадлежит игроку, либо требуемое кол-во больше чем доступное</exception>
	public void BuyStock(Stock stock, int amount)
	{
		// TODO: переделать сводку
		if (stock == null)
			throw new Exception("Пустой объект ценной бумаги! Stock is null!");

		decimal sum = CalcSum(stock, amount);
		decimal comission = CalcComission(stock, amount);

		AssetType assetType = stock is Shares ? AssetType.Shares : AssetType.Bonds;
		AssetPurchase asset = Managers.Player.FindAsset(assetType, stock);
		try
		{
			if (asset == null)
			{
				// Добавление акций в список активов игрока
				asset = new AssetPurchase(stock.Ticket);
				Managers.Player.AddAsset(assetType, asset);
			}

			if (Managers.Player.Wallet[stock.Currency] < sum + comission)
			{
				// Messenger.Broadcast(GameNotice.LOW_MONEY_IN_WALLET); // TODO: не работает
				Managers.UI.ShowLowMoneyNotice();
				throw new Exception("Недостаточно средств для покупки ценной бумаги");
			}

			asset.Add(amount);
		}
		catch
		{
			if (asset.Amount == 0)
			{
				Managers.Player.RemoveAsset(assetType, asset);
			}

			Messenger.Broadcast(StockExchangeNotice.STOCK_BUY_FAILED);
			return;
		}

		Managers.Player.ChangeWalletMoney(stock.Currency, -(sum + comission)); // снятие средств со счета

		Messenger.Broadcast(StockExchangeNotice.STOCK_BUY_ACCESS);
		Controllers.Transactions.AddTransaction(new Transaction()
		{
			Date = Managers.Game.Date,
			ItemId = stock.Ticket,
			Price = stock.Price,
			Comission = comission,
			Currency = stock.Currency,
			Quantity = amount,
			Type = TransactionType.Buy
		});
	}

	/// <summary>
	/// Продажа ценной бумаги
	/// </summary>
	/// <param name="stock">Ценная бумага</param>
	/// <param name="amount">Кол-во</param>
	public void SellStock(Stock stock, int amount)
	{
		// Отлов исключений
		if (stock == null)
			throw new Exception("Пустой объект ценной бумаги! Stock is null!");


		AssetType assetType = stock is Shares ? AssetType.Shares : AssetType.Bonds;
		AssetPurchase asset = Managers.Player.FindAsset(assetType, stock);
		try
		{
			if (asset == null)
				throw new Exception("Таких активов нет у игрока!");

			asset.Remove(amount);
		}
		catch
		{
			Messenger.Broadcast(StockExchangeNotice.STOCK_BUY_FAILED);
			return;
		}

		// Удаление ценной бумаги из активов игрока, если ее не осталось
		if (asset.Amount == 0)
		{
			Managers.Player.RemoveAsset(assetType, asset);
		}

		decimal sum = stock.Price * amount;
		decimal comission = sum * comissionRate;
		decimal tax = this.CalcTax(stock, amount);
		Managers.Player.ChangeWalletMoney(stock.Currency, sum - comission - tax); // зачисление средств со счета

		Messenger.Broadcast(StockExchangeNotice.STOCK_BUY_ACCESS);
		Controllers.Transactions.AddTransaction(new Transaction()
		{
			Date = Managers.Game.Date,
			ItemId = stock.Ticket,
			Price = stock.Price,
			Comission = comission,
			Currency = stock.Currency,
			Quantity = amount,
			Type = TransactionType.Sell
		});
	}


	/// <summary>
	/// Листинг(добавление) ценной бумаги на биржу
	/// </summary>
	/// <param name="stockTicket">Тикет ценной бумаги</param>
	public void AddNewStock(StockType stockType, string stockTicket)
	{
		//Debug.Log($"Листинг {stockType} {stockTicket}");
		_model.Stocks[stockType].Add(stockTicket);
		// Сортировка списков
		try
		{
			if (stockType == StockType.Shares)
				_model.Stocks[stockType] = _model.Stocks[stockType].OrderBy(x => (Controllers.Assets.FindAsset(x) as Shares).Currency).ToList();
			if (stockType == StockType.Bonds)
				_model.Stocks[stockType] = _model.Stocks[stockType].OrderBy(x => (Controllers.Assets.FindAsset(x) as Bond).Currency).ToList();
		}
		catch { }
	}

	/// <summary>
	/// Делистинг(удаление) ценной бумаги с биржи
	/// </summary>
	/// <param name="stock">Ценная бумага</param>
	public void RemoveStock(Stock stock)
	{
		switch (stock)
		{
			case Shares:
				_model.Stocks[StockType.Shares].Remove(stock.Ticket);
				break;
			case Bond:
				_model.Stocks[StockType.Bonds].Remove(stock.Ticket);
				break;
		}

		Messenger.Broadcast(StockExchangeNotice.STOCK_DELISTING);
	}

	/// <summary>
	/// Поиск ценной бумаги по тикету. Если ценная бумага не торгуется на бирже или ее вообще нет, вернет null.
	/// </summary>
	/// <param name="type">Тип ценной бумаги</param>
	/// <param name="ticket">Тикет ценной бумаги</param>
	/// <returns>Ценная бумага</returns>
	public Stock FindStock(StockType type, string ticket)
	{
		string stockTicket = _model.Stocks[type].Find(x => x == ticket);
		if (stockTicket == null) return null;
		return Controllers.Assets.FindAsset(stockTicket) as Stock;
	}




	/// <summary>
	/// Обновление цены указанной ценной бумаги
	/// </summary>
	/// <param name="stock">Ценная бумага</param>
	/// <param name="price">Цена</param>
	public void UpdateStockPrice(Stock stock, decimal price)
	{
		stock.Price = price;
	}

	/// <summary>
	/// Обновить количество указанной ценной бумаги
	/// </summary>
	/// <param name="stock">Ценная бумага</param>
	/// <param name="amount">Кол-во</param>
	public void UpdateStockAmount(Stock stock, int amount)
	{
		stock.Amount = amount;
	}

	/// <summary>
	/// Обновление имени биржи
	/// </summary>
	/// <param name="name">Имя</param>
	public void UpdateExchangeName(string name)
	{
		_model.Name = name;
	}


	private void DayChangeTrigger()
    {
		Messenger.Broadcast(DataNotice.STOCK_EXCHANGE_UPDATED);
	}


    public object GetDataCopy()
    {
        return _model.Clone();
    }
}