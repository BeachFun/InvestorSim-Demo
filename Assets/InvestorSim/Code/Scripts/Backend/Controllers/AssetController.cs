using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер всех активов в игре. Содержит средства поиска активов и изменения цен активов.
/// </summary>
public class AssetController : MonoBehaviour, IGameController, IDataCopable
{
    // TODO: исправить не логичность класса. Своство Data предоставляет данные для изменения наружному коду.
    // TODO: Добавить оповещения в контроллер

    [Header("Настройка ценообразования акций")]
    [Space]
    [Tooltip("Граница отклонения акции от цены в %")]
    [Range(1f, 10f)] public float sharesDispersionBorder;
    [Tooltip("Граница отклонения акции от цены за день в %")]
    [Range(.5f, 2f)] public float sharesDailyDispersionBorder;

    [Header("Настройка ценообразования облигаций")]
    [Space]
    [Tooltip("Граница отклонения облигаций от цены в %")]
    [Range(1f, 2f)] public float bondDispersionBorder;
    [Tooltip("Граница отклонения облигаций от цены за день в %")]
    [Range(.25f, 1f)] public float bondDailyDispersionBorder;


    public ControllerStatus status { get; private set; }

    /// <summary>
    /// Активы в игре | Key - asset name, Value - (price / date change)
    /// </summary>
    public Dictionary<IAsset, List<(decimal, DateTime)>> Data
    {
        get => _model.Assets;
    }


    private AssetsModel _model;


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
        status = ControllerStatus.Initializing;

        yield return null;

        // Если на момент запуска нет данных, то создаются данные по-умолчанию
        if (_model is null) _model = new AssetsModel();

        //Messenger.AddListener<string>(StartupNotice.PRIMARY_PRICES_GENERATION, DayChangeTrigger);
        Messenger.AddListener(GameNotice.DAY_CHANGE, DayChangeTrigger);

        status = ControllerStatus.Started;
    }

    public void UpdateData(AssetsModel data)
    {
        _model = data;
    }

    #endregion


    /// <summary>
    /// Добавляет актив в список активов в игре
    /// </summary>
    /// <param name="asset"></param>
    public void AddAsset(IAsset asset)
    {
        _model.AddAsset(asset);
        if (Managers.Game.GameStarted && Controllers.Corporations.IsStarted)
            Messenger.Broadcast(DataNotice.ASSETS_MODEL_UPDATED);
    }

    /// <summary>
    /// Поиск актива по тикету, если его нет вернет null
    /// </summary>
    /// <param name="ticket">Тикет актива</param>
    public IAsset FindAsset(string ticket)
    {
        if (!this.Data.Keys.Any(x => x.Ticket == ticket)) return null;

        return this.Data.Keys.Where(x => x.Ticket == ticket).First();
    }

    /// <summary>
    /// Возвращает наименование актива, если его нет вернет null
    /// </summary>
    /// <param name="ticket">Тикет актива</param>
    public string GetName(string ticket)
    {
        IAsset asset = this.FindAsset(ticket);

        if (asset == null) return null;

        return asset switch
        {
            Bond => (asset as Bond).Name,
            Shares => (asset as Shares).CorporationName,
            _ => ""
        };
    }


    #region Методы изменения цен

    /// <summary>
    /// Изменяет рыночную стоимость актива
    /// </summary>
    /// <param name="asset">Актив</param>
    /// <param name="delta">Величина изменения цены</param>
    public void ChangePrice(IAsset asset, decimal delta)
    {
        this.ChangePrice(asset, delta, Managers.Game.Date);
    }
    /// <summary>
    /// Изменяет рыночную стоимость актива c указанием дня, в котором была изменена цена
    /// </summary>
    /// <param name="asset">Актив</param>
    /// <param name="delta">Величина изменения цены</param>
    /// <param name="date">День изменения цены</param>
    public void ChangePrice(IAsset asset, decimal delta, DateTime date)
    {
        if (asset is null) throw new Exception("Методу был передан пустой IAsset");

        asset.ChangePrice(delta);
        this.Data[asset].Add((asset.Price, date));
    }

    /// <summary>
    /// Изменяет рыночную стоимость актива в процентном соотношении
    /// </summary>
    /// <param name="asset">Актив</param>
    /// <param name="deltaRate">Процент измения от текущей цены</param>
    public void ChangeRatePrice(IAsset asset, decimal deltaRate)
    {
        this.ChangeRatePrice(asset, deltaRate, Managers.Game.Date);
    }
    /// <summary>
    /// Изменяет рыночную стоимость актива в процентном соотношении c указанием дня, в котором была изменена цена
    /// </summary>
    /// <param name="asset">Актив</param>
    /// <param name="deltaRate">Процент измения от текущей цены</param>
    /// <param name="date">День изменения цены</param>
    public void ChangeRatePrice(IAsset asset, decimal deltaRate, DateTime date)
    {
        if (asset is null) throw new Exception("Методу был передан пустой IAsset");
        if (deltaRate < -100) throw new Exception("Ошибка изменения цены. Нельзя вычесть больше, чем 100% от цены");

        asset.Price *= (1 + deltaRate * (decimal)0.01);
        this.Data[asset].Add((asset.Price, date));
    }

    /// <summary>
    /// Изменяет реальную стоимость актива
    /// </summary>
    /// <param name="asset">Актив</param>
    /// <param name="delta">Величина изменения цены</param>
    public void ChangeFairPrice(IAsset asset, decimal delta)
    {
        if (asset is null) throw new Exception("Методу был передан пустой IAsset");
        asset.ChangeFairPrice(delta);
    }

    /// <summary>
    /// Изменяет реальную стоимость актива в процентном соотношении
    /// </summary>
    /// <param name="asset">Актив</param>
    /// <param name="deltaRate">Процент измения от текущей цены</param>
    public void ChangeRateFairPrice(IAsset asset, decimal deltaRate)
    {
        if (asset is null) throw new Exception("Методу был передан пустой IAsset");
        if (deltaRate < -100) throw new Exception("Ошибка изменения цены. Нельзя вычесть больше, чем 100% от цены");

        asset.FairPrice *= (1 + deltaRate * (decimal)0.01);
    }

    #endregion

    #region Методы статистики цен

    /// <summary>
    /// Изменение цены за день
    /// <param name="asset">Актив</param>
    /// </summary>
    public decimal GetDay1Delta(IAsset asset)
    {
        if (asset is null) throw new Exception("Методу был передан пустой IAsset");

        List<(decimal, DateTime)> data = this.Data[asset];
        return data.Count > 1 ? data[^1].Item1 - data[^2].Item1 : 0;
    }
    /// <summary>
    /// Процент изменения цены за день
    /// <param name="asset">Актив</param>
    /// </summary>
    public decimal GetDay1DeltaPrecent(IAsset asset)
    {
        if (asset is null) throw new Exception("Методу был передан пустой IAsset");

        List<(decimal, DateTime)> data = this.Data[asset];
        return data.Count > 1 ? this.GetDay1Delta(asset) / data[^2].Item1 : 0;
    }

    /// <summary>
    /// Предыдущая рыночная цена
    /// <param name="asset">Актив</param>
    /// </summary>
    public decimal GetPrevPrice(IAsset asset)
    {
        if (asset is null) throw new Exception("Методу был передан пустой IAsset");

        return this.Data[asset][^2].Item1;
    }

    /// <summary>
    /// История цен определенного актива
    /// </summary>
    /// <param name="asset"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public List<(float, DateTime)> GetPriceStory(IAsset asset)
    {
        if (asset is null) throw new Exception("Методу был передан пустой IAsset");

        return this.Data[asset].Select(x => ((float)x.Item1, x.Item2)).ToList();
    }

    #endregion


    private void DayChangeTrigger()
    {
        //Debug.Log("Asset controller calc started");

        // Запуск действий у активов при смене дня
        {
            List<IAsset> assets = this.Data.Keys.ToList();
            foreach(var asset in assets)
            {
                if (asset is Bond)
                {
                    (asset as Bond).DayChangeTrigger();
                }

                if (asset is Shares)
                {
                    (asset as Shares).DayChangeTrigger();
                }
            }
        }

        // Изменение цен
        {
            System.Random rnd = new System.Random();
            List<string> stocksOnExchange;

            // Изменение цен на акции
            stocksOnExchange = Controllers.StockExchange.StockList[StockType.Shares];
            foreach (string stock in stocksOnExchange)
            {
                this.ChangePrice(stock, this.sharesDailyDispersionBorder, this.sharesDispersionBorder, rnd);
            }

            // Изменение цен на облигации
            stocksOnExchange = Controllers.StockExchange.StockList[StockType.Bonds];
            foreach (string stock in stocksOnExchange)
            {
                this.ChangePrice(stock, this.bondDailyDispersionBorder, this.bondDispersionBorder, rnd);
            }
        }

        Messenger.Broadcast(DataNotice.ASSETS_MODEL_UPDATED);
    }

    /// <summary>
    /// Изменяет цену ценной бумаги
    /// </summary>
    /// <param name="dayDispersionBorder">Границы изменения цены за день</param>
    /// <param name="dispersionBorder">Границы изменения цены от ее реальной цены</param>
    /// <param name="rnd">System.Random - нужен для меньшего числа, потребления памяти</param>
    private void ChangePrice(string stockTicket, float dayDispersionBorder, float dispersionBorder, System.Random rnd)
    {
        decimal dispersion, deltaRate, priceImpulse = 0, fairPriceImpulse = 0;
        IAsset asset = this.FindAsset(stockTicket);
        if (asset is null) throw new Exception("не существующий актив");
        Stock stock = asset as Stock;

        // Вычисление обычного случайного движения цены
        deltaRate = (decimal)UnityEngine.Random.Range(0f, dayDispersionBorder);
        dispersion = Math.Abs((stock.Price / stock.FairPrice) - 1);

        if (rnd.Next(0, 2) == 0) deltaRate = -deltaRate;

        if (dispersion > (decimal)dispersionBorder / 100) // когда цена вышла за границу
        {
            //Debug.Log("Цена вышла за рамки, обратное движение запущено");
            if (stock.Price - stock.FairPrice > 0) // за верхнюю границу
                deltaRate = -Math.Abs(deltaRate);
            else // за нижнюю границу
                deltaRate = Math.Abs(deltaRate);
        }
        //<<<

        // Вычисление импульсивного движения цены
        List<News> newsList = Controllers.News.GetNewsList(stock);
        foreach (News news in newsList)
        {
            if (news.ReleaseDate == Managers.Game.Date) continue;
            //Debug.Log(" | " + newsList.GetHashCode());
            decimal impulseStep = news.Related[stock.Ticket].CalcCurrentPower();
            priceImpulse += impulseStep;
            if (news.Title == "Выплата дивидендов" || news.Title == "Финансовый отчет") // TODO: Временное решение
                fairPriceImpulse += impulseStep;
        }

        this.ChangeRatePrice(stock, deltaRate + priceImpulse);
        this.ChangeRateFairPrice(stock, fairPriceImpulse);
    }

    public void LogInfo(IAsset asset)
    {
        Debug.Log(string.Format("Day: {0} | Ticket: {1}\nPrice = {2:f2} | FairPrice = {3:f2}\nDayDelta: {4:f2} | {5:p2}\nDispersion: {7}{6:f2}",
            Managers.Game.Date.ToMyFormat(),
            asset.Ticket,
            asset.Price,
            asset.FairPrice,
            this.GetDay1Delta(asset),
            this.GetDay1DeltaPrecent(asset),
            Math.Abs((asset.Price / asset.FairPrice) - 1),
            asset.Price - asset.FairPrice > 0 ? '+' : '-'
            ));
    }


    public object GetDataCopy()
    {
        return _model.Clone();
    }
}