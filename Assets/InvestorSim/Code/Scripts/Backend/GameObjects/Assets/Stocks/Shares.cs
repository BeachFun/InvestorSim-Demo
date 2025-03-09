using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]

/// <summary>
/// Объект акций, содержащий сведения об акциях
/// </summary>
public class Shares : Stock, IGameObject, ICloneable
{
	public Shares()
	{
		this.Dividends = new List<Dividend>();
	}
	public Shares(StockInfo info, ShareType type, MovingForce force, Corporation corporation)
		: base(info.Ticket, info.Price, info.Currency, info.Issue, info.Amount, info.IssueDate, info.StockExchange)
	{
		this.Type = type;
		this.Force = force;
		this.CorporationName = corporation.Name;

		this.Dividends = new List<Dividend>();
		this._movePriceStep = (decimal)force / 365;
	}
	/// <summary>
	/// Создает объект акций. Выпуск акций
	/// </summary>
	/// <param name="ticket">Тикет акции (наименование)</param>
	/// <param name="issue">Общее кол-во акций</param>
	/// <param name="count">Кол-во акций в свободном обращении</param>
	/// <param name="stockExchanges">Биржа/Биржи на кот будет торговаться</param>
	/// <param name="corporation">Корпорация эмитент</param>
	public Shares(ShareType type, string ticket,
				decimal price, string currency,
				int issue, int count,
				DateTime issueDate,
				MovingForce force,
				List<string> stockExchanges,
				Corporation corporation)
		: base(ticket, price, currency, issue, count, issueDate, stockExchanges)
	{
		this.Type = type;
		this.Force = force;
		this.CorporationName = corporation.Name;

		this.Dividends = new List<Dividend>();
		this._movePriceStep = (decimal)force / 53;
	}


    #region Автосвойства класса

    /// <summary>
    /// Вид акций компании
    /// </summary>
    public ShareType Type { get; set; }
	/// <summary>
	/// Движущая сила акции
	/// </summary>
	public MovingForce Force { get; set; }
	/// <summary>
	/// Наименование корпорации, выпустившая акции
	/// </summary>
	public string CorporationName { get; set; }
	/// <summary>
	/// Список всех дивидендов за историю акции
	/// </summary>
	public List<Dividend> Dividends { get; private set; }

	/// <summary>
	/// Текущая годовая ставка дивидендов в денежном эквиваленте
	/// </summary>
	public decimal DividendRate { get; private set; }
	/// <summary>
	/// Является ли игрок держателем этих акций на момент выплаты дивидендов
	/// </summary>
	public bool IsHolder { get; private set; }

    #endregion


    private decimal _movePriceStep; // шаг изменения цены акции за день. исходит из MovingForce


	/// <summary>
	/// Добавление выплаты двидиендов
	/// </summary>
	/// <param name="dividend">Инвормация о дивидендах</param>
	public void AddDividend(Dividend dividend)
	{
		Dividends.Add(dividend);

		// Обновляет поле DividendRate
		decimal totalDividends = Dividends.Sum(x => x.DividendValue);
		decimal dividendRate = totalDividends / Price;

		DividendRate = dividendRate;
	}

	/// <summary>
	/// Возвращает ближайшую выплату дивидендов
	/// </summary>
	/// <returns>Дивиденд</returns>
	public Dividend? GetNextDividend()
	{
		DateTime currentDate = Managers.Game.Date;
		foreach (var dividend in this.Dividends)
		{
			if (currentDate <= dividend.PayDate)
				return dividend;
		}
		return null;
	}

	/// <summary>
	/// Возвращает список акутальных выплат дивидендов
	/// </summary>
	public List<Dividend> GetActualDividends()
    {
		List<Dividend> result = new List<Dividend>();

		DateTime currentDate = Managers.Game.Date;
		foreach (var dividend in this.Dividends)
		{
			if (currentDate <= dividend.PayDate)
				result.Add(dividend);
		}

		return result;
	}



	internal void DayChangeTrigger()
	{
		// Немного сдвигает справедливую цену
		if(Managers.Game.Date.DayOfWeek == DayOfWeek.Monday)
			this.FairPrice += _movePriceStep;

		// Сдвиг справедливой цены на выплату дивидендов (по шагам каждый день)
		List<Dividend> dividends = this.GetActualDividends();
		if (dividends.Count > 0)
			foreach (Dividend dividend in dividends)
				this.FairPrice += dividend.DividendValue / (dividend.CutOffDate - dividend.DeclarationDate).Days;


		Dividend? nextDividend = this.GetNextDividend();
		
		if (nextDividend is null) 
			return;

		// Див отсечка
		if (nextDividend.Value.CutOffDate == Managers.Game.Date)
		{
			if (Managers.Player.FindAsset(AssetType.Shares, this) is not null)
				this.IsHolder = true;
			else
				this.IsHolder = false;
		}

		// Проверка дня выплаты дивидендов
		if (nextDividend.Value.PayDate == Managers.Game.Date)
		{
			Messenger<Shares>.Broadcast(StockExchangeNotice.DIVIDEND_PAYMENT, this);
		}
	}


    public object Clone()
    {
		//UnityEngine.Debug.Log($"share save started {this.CorporationName}");
		var data = new Shares()
		{
			Amount = this.Amount,
			CorporationName = this.CorporationName,
			Currency = this.Currency,
			Description = this.Description,
			DividendRate = this.DividendRate,
			FairPrice = this.FairPrice,
			Force = this.Force,
			IsHolder = this.IsHolder,
			Issue = this.Issue,
			IssueDate = this.IssueDate,
			Price = this.Price,
			Ticket = this.Ticket,
			Type = this.Type,
			_movePriceStep = this._movePriceStep,
			StockExchange = this.StockExchange is not null ? this.StockExchange.ToList() : null,
			Dividends = this.Dividends is not null ? this.Dividends.ToList() : null
        };
		//UnityEngine.Debug.Log($"share save end {this.CorporationName}");
		return data;
    }
}