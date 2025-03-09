/// <summary>
/// Периодичность оплаты/выплат
/// </summary>
public enum PaymentFrequency
{
    /// <summary>
    /// Без оплаты
    /// </summary>
    None,
    /// <summary>
    /// Один раз
    /// </summary>
    Once,
    /// <summary>
    /// Ежедневно
    /// </summary>
    Daily = 3650,
    /// <summary>
    /// Еженедельно
    /// </summary>
    Weekly = 530,
    /// <summary>
    /// Ежемесячно
    /// </summary>
    Monthly = 120,
    /// <summary>
    /// Ежеквартально, раз в квартал
    /// </summary>
    Quarterly = 40,
    /// <summary>
    /// Раз в полгода, два раза в год
    /// </summary>
    BiAnnually = 20,
    /// <summary>
    /// Ежегодно, каждый год.
    /// </summary>
    Annually = 10
}