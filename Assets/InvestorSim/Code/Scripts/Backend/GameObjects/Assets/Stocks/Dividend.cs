using System;

[Serializable]

/// <summary>
/// Содержит информацию о выплате дивидендов
/// </summary>
public struct Dividend
{
    /// <summary>
    /// Величина дивиденда
    /// </summary>
    public decimal DividendValue { get; set; }
    /// <summary>
    /// Дата объявления выплат
    /// </summary>
    public DateTime DeclarationDate { get; set; }
    /// <summary>
    /// Дата выплаты дивидендов
    /// </summary>
    public DateTime PayDate { get; set; }
    /// <summary>
    /// Дата дивидендной отсечки
    /// </summary>
    public DateTime CutOffDate { get; set; }
}
