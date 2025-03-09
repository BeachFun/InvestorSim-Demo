using System;

[Serializable]

/// <summary>
/// Структура данных, представляющая данные транзакции
/// </summary>
public struct Transaction
{
    /// <summary>
    /// Тип транзакции (купля/продажа/обмен)
    /// </summary>
    public TransactionType Type { get; set; }
    /// <summary>
    /// Товар
    /// </summary>
    public string ItemId { get; set; }
    /// <summary>
    /// Количество товара
    /// </summary>
    public decimal Quantity { get; set; }
    /// <summary>
    /// Цена за единицу товара
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Комиссия за транзакцию
    /// </summary>
    public decimal Comission { get; set; }
    /// <summary>
    /// Валюта транзакции
    /// </summary>
    public string Currency { get; set; }
    /// <summary>
    /// Дата транзакции
    /// </summary>
    public DateTime Date { get; set; }
}