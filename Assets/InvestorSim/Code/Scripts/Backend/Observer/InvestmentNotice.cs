/// <summary>
/// События вклада
/// </summary>
internal class InvestmentNotice
{
    /// <summary>
    /// День выплаты
    /// </summary>
    internal const string PAY_DATE = "INVESTMENT_NOTICE_PAY_DATE";
    /// <summary>
    /// День погашения
    /// </summary>
    internal const string MATURITY_DATE = "INVESTMENT_NOTICE_MATURITY_DATE";

    /// <summary>
    /// Выплата провалилась
    /// </summary>
    internal const string PAYOUT_FAILED = "INVESTMENT_NOTICE_PAYOUT_FAILED";
    /// <summary>
    /// Выплата совершена
    /// </summary>
    internal const string PAYOUT_ACCESS = "INVESTMENT_NOTICE_PAYOUT_ACCESS";
    /// <summary>
    /// Вклад не возвращен
    /// </summary>
    internal const string PAYOUT_NOMINAL_FAILED = "INVESTMENT_NOTICE_PAYOUT_NOMINAL_FAILED";
    /// <summary>
    /// Вклад возвращен
    /// </summary>
    internal const string PAYOUT_NOMINAL_ACCESS = "INVESTMENT_NOTICE_PAYOUT_NOMINAL_ACCESS";
}
