using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]

/// <summary>
/// Приобретение. Объекты класса хранят данные о покупке актива и предоставляет средства для расчетов
/// </summary>
public class AssetPurchase : ICloneable
{
    private AssetPurchase()
    {
        Buys = new List<PlayerAssetInfo>();
    }
    public AssetPurchase(string assetTicket) : this()
    {
        this.Asset = assetTicket;
    }


    #region Properties

    /// <summary>
    /// Тикет актива
    /// </summary>
    public string Asset { get; set; }
    /// <summary>
    /// Список покупок данного актива. Точечные приобретения / покупки (кол-во, цена, дата, доход)
    /// </summary>
    public List<PlayerAssetInfo> Buys { get; set; }

    /// <summary>
    /// Рыночная стоимость активов
    /// </summary>
    public decimal Cost { get => Controllers.Assets.FindAsset(this.Asset).Price * this.Amount; }
    /// <summary>
    /// Кол-во актива
    /// </summary>
    public int Amount { get => Buys.Sum(x => x.Amount); }
    /// <summary>
    /// Цены покупок
    /// </summary>
    public List<decimal> BuyPrices { get => Buys.Select(x => x.Price).ToList(); }
    /// <summary>
    /// Фактический доход от активов. Бывает у облигаций, акций, компаний и определенной формы недвижемости (аренда например)
    /// </summary>
    public decimal Profit { get => Buys.Sum(x => x.Profit); }

    #endregion


    #region Calc Methonds

    /// <summary>
    /// Вычисляет доход от актива. Если актив не приносит дохода, то будет возвращено ProfitPrice (изменение цены дохода)
    /// </summary>
    /// <returns>Доход от актива</returns>
    public decimal CalcProfit()
    {
        decimal? extraProfit = 0; // дополнительный доход

        IAsset asset = Controllers.Assets.FindAsset(this.Asset);
        switch (asset)
        {
            // Расчет НКД
            case Bond:
                extraProfit = (asset as Bond).Type == BondType.Coupon ? Controllers.StockExchange.CalcCouponValue(asset as Bond) : 0;
                break;
        }

        if (extraProfit.HasValue)
            return this.CalcProfitPrice() + this.Profit + extraProfit.Value;
        else
            return this.CalcProfitPrice() + this.Profit;
    }
    /// <summary>
    /// Вычисляет процент дохода от стоимости актива
    /// </summary>
    /// <returns>Процент дохода</returns>
    public decimal CalcProfitPrecent()
    {
        return this.CalcProfit() / (this.CalcAvgPrice() * this.Buys.Sum(x => x.Amount));
    }
    /// <summary>
    /// Вычисляет доход от изменения цены актива
    /// </summary>
    /// <returns>Доход</returns>
    public decimal CalcProfitPrice()
    {
        return (Controllers.Assets.FindAsset(this.Asset).Price - this.CalcAvgPrice()) * Amount;
    }
    /// <summary>
    /// Вычисляет процент от дохода от изменения цены актива
    /// </summary>
    /// <returns>Доход в процентах</returns>
    public decimal CalcProfitPricePrecent()
    {
        return (Controllers.Assets.FindAsset(this.Asset).Price / this.CalcAvgPrice() - 1);
    }
    /// <summary>
    /// Вычисляет среднюю цену купленого/купленных активов.
    /// </summary>
    /// <returns>Средняя цена покупки актива/активов</returns>
    public decimal CalcAvgPrice()
    {
        return Buys.Sum(x => x.Amount * x.Price) / Buys.Sum(x => x.Amount);
    }

    #endregion


    /// <summary>
    /// Добавление определенного кол-ва активов
    /// </summary>
    /// <param name="amount">Кол-во</param>
    /// <exception cref="Exception">Возникает при недостаточном кол-ве актива в свободном обращении</exception>
    public void Add(int amount)
    {
        if (amount <= 0)
            throw new Exception("Некорректный ввод числа!");
        if (amount > Controllers.Assets.FindAsset(this.Asset).Amount)
            throw new Exception("Недостаточное кол-во актива в свободном обращении");

        Controllers.Assets.FindAsset(this.Asset).Amount -= amount; // отнимание кол-во актива из свободного обращения

        Buys.Add(new PlayerAssetInfo(amount, Controllers.Assets.FindAsset(this.Asset).Price, Managers.Game.Date, 0)); // добавление покупки

        Messenger.Broadcast(PlayerNotice.ASSET_PURCHASE_UPDATED);
    }

    /// <summary>
    /// Удаление определенного кол-ва активов
    /// </summary>
    /// <param name="amount">Кол-во</param>
    /// <exception cref="Exception">Возникает при недостаточном кол-ве актива у игрока</exception>
    public void Remove(int amount)
    {
        if (amount <= 0)
            throw new Exception("Некорректный ввод числа!");
        if (amount > this.Amount)
            throw new Exception("Недостаточное кол-во актива у игрока!");

        Controllers.Assets.FindAsset(this.Asset).Amount += amount; // добавление кол-ва актива в свободное обращение

        // Анулирование покупок
        for (int i = 0; i < Buys.Count; i++)
        {
            amount -= Buys[i].Amount;
            Buys[i] = new PlayerAssetInfo(0, Buys[i].Price, Buys[i].Date, Buys[i].Profit); // TODO: делегировать изменения в сам класс
            if (amount < 0)
            {
                Buys[i] = new PlayerAssetInfo(Math.Abs(amount), Buys[i].Price, Buys[i].Date, Buys[i].Profit); // TODO: делегировать изменения в сам класс
                break;
            }
        }
        Buys = Buys.Where(x => x.Amount != 0).ToList();

        Messenger.Broadcast(PlayerNotice.ASSET_PURCHASE_UPDATED);
    }


    /// <summary>
    /// Возвращение данных объекта
    /// </summary>
    public Dictionary<string, object> GetData()
    {
        Dictionary<string, object> pairs = new();
        pairs.Add("1", Controllers.Assets.FindAsset(this.Asset).Ticket);
        pairs.Add("2", this.Buys);

        return pairs;
    }


    public object Clone()
    {
        return new AssetPurchase()
        {
            Asset = this.Asset,
            Buys = this.Buys.ToList()
        };
    }
}
