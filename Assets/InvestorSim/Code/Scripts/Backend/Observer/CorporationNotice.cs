/// <summary>
/// Уведомления связанные с корпорациями
/// </summary>
public class CorporationNotice
{
    /// <summary>
    /// Объявление о выплате дивидендов
    /// </summary>
    public const string DIVIDEND_ANNOUNCED = "CORPORATION_NOTICE_" + "DIVIDEND_ANNOUNCED";
    /// <summary>
    /// Объявление об отмене выплаты дивидендов
    /// </summary>
    public const string DIVIDEND_ANNOUNCED_CANCELED = "CORPORATION_NOTICE_" + "DIVIDEND_ANNOUNCED_CANCELED";
    /// <summary>
    /// Выход финансового отчета
    /// </summary>
    public const string FINANCE_STATEMENT_RELEASED = "CORPORATION_NOTICE_" + "FINANCE_STATEMENT_RELEASED";
    /// <summary>
    /// Судебные разбирательства
    /// </summary>
    public const string COURT_PROCEEDINGS = "CORPORATION_NOTICE_" + "COURT_PROCEEDINGS"; // TODO: временно
}