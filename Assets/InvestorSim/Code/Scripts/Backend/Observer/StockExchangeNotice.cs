public static class StockExchangeNotice
{
	public const string SHARE_CARD_CLICK = "STOCK_EXCHANGE_SHARE_CARD_CLICK"; // -

	/// <summary>
	/// Ценная бумага была удалена с биржи
	/// </summary>
	public const string STOCK_DELISTING = "STOCK_DELISTING";

	public const string STOCK_BUY_ACCESS = "STOCK_BUY_ACCESS";
	public const string STOCK_BUY_FAILED = "STOCK_BUY_FAILED";

	public const string STOCK_SELL_ACCESS = "STOCK_SELL_ACCESS";
	public const string STOCK_SELL_FAILED = "STOCK_SELL_FAILED";

	/// <summary>
	/// Выплата купона
	/// </summary>
	public const string COUPON_PAYMENT = "COUPON_PAYMENT";
	public const string COUPON_RECEIPT = "COUPON_RECEIPT";
	/// <summary>
	/// Выплата дивидендов
	/// </summary>
	public const string DIVIDEND_PAYMENT = "DIVIDEND_PAYMENT";
	public const string DIVIDEND_RECEIPT = "DIVIDEND_RECEIPT";
	/// <summary>
	/// Дивидендный гэп
	/// </summary>
	public const string DIVIDEND_GAP = "DIVIDEND_GAP";

	/// <summary>
	/// Облигация была погашена. Выплата номинал
	/// </summary>
	public const string BONDS_NOMINAL_PAYOUT = "BONDS_NOMINAL_PAYOUT";

	public const string BONDS_MATURITY_DATE = "BONDS_MATURITY_DATE";
}