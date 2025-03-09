using System;

[Serializable]

/// <summary>
/// Структура данных, представляющее информацию об приобретении актива и его доходности
/// </summary>
public struct PlayerAssetInfo
{
    public PlayerAssetInfo(int amount, decimal price, DateTime date, decimal profit)
    {
        Amount = amount;
        Price = price;
        Date = date;
        Profit = profit;
    }

    /// <summary>
    /// 1. Кол-во актива
    /// </summary>
    public int Amount
    {
        get;
        set;
    }

    /// <summary>
    /// 2. Цена актива
    /// </summary>
    public decimal Price
    {
        get;
        set;
    }

    /// <summary>
    /// 3. Дата приобретения актива
    /// </summary>
    public DateTime Date
    {
        get;
        set;
    }

    /// <summary>
    /// 4. Доход от актива
    /// </summary>
    public decimal Profit
    {
        get;
        set;
    }
}
