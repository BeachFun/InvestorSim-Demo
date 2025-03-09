using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Диспетчер управления данными игрока
/// </summary>
public class PlayerManager : MonoBehaviour, IGameManager, IDataCopable
{
    /// <summary>
    /// Статус менеджера
    /// </summary>
    public ManagerStatus status { get; private set; }

    public Dictionary<string, decimal> Wallet { get => _model.Wallet; }
    /// <summary>
    /// Активы игрока
    /// </summary>
    public Dictionary<AssetType, List<AssetPurchase>> Assets { get => _model.Assets; }
    /// <summary>
    /// Вклады игрока
    /// </summary>
    public List<Investment> Investments { get => _model.Investments; }
    /// <summary>
    /// Должники игрока
    /// </summary>
    public List<Dept> Deptors { get => _model.Deptors; }


    private PlayerModel _model;


    #region Методы запуска и Инициализации

    private void OnDestroy()
    {
        if (status == ManagerStatus.Started)
        {
            Messenger.RemoveListener(PlayerNotice.ASSET_PURCHASE_UPDATED, this.AssetPurchaseUpdatedTrigger);
            Messenger.RemoveListener(PlayerNotice.INVESTMENT_UPDATED, this.InvestmentUpdatedTrigger);
            Messenger.RemoveListener(PlayerNotice.DEPT_UPDATED, this.DeptUpdatedTrigger);
            Messenger.RemoveListener(GameNotice.DAY_CHANGE, this.DayChangeTrigger);
        }
    }

    public IEnumerator Startup()
    {
        Messenger.Broadcast(StartupNotice.PLAYER_MANAGER_STARTING);
        status = ManagerStatus.Initializing;

        yield return null;

        // Если на момент запуска нет данных, то создаются данные по-умолчанию
        if (_model is null) _model = new PlayerModel();

        Messenger.AddListener(PlayerNotice.ASSET_PURCHASE_UPDATED, this.AssetPurchaseUpdatedTrigger);
        Messenger.AddListener(PlayerNotice.INVESTMENT_UPDATED, this.InvestmentUpdatedTrigger);
        Messenger.AddListener(PlayerNotice.DEPT_UPDATED, this.DeptUpdatedTrigger);
        Messenger.AddListener(GameNotice.DAY_CHANGE, this.DayChangeTrigger);

        status = ManagerStatus.Started;
        Messenger.Broadcast(StartupNotice.PLAYER_MANAGER_STARTED);
    }

    public void UpdateData(PlayerModel model)
    {
        _model = model;
    }

    #endregion

    /// <summary>
    /// Изменяет кол-во средств указанной валюты на дельту
    /// </summary>
    /// <param name="currency">Валюта</param>
    /// <param name="delta">Величина изменения</param>
    /// <exception cref="Exception">Вызывается при недостаточном кол-ве средств</exception>
    public void ChangeWalletMoney(string currency, decimal delta)
    {
        if (!_model.Wallet.ContainsKey(currency))
        {
            return;
        }

        if (_model.Wallet[currency] + delta >= 0)
        {
            _model.Wallet[currency] += delta;
        }
        else
        {
            // Messenger.Broadcast(GameNotice.LOW_MONEY_IN_WALLET); // TODO: не работает
            Managers.UI.ShowLowMoneyNotice();
            throw new Exception("На счете недостаточно средств!");
        }

        Messenger.Broadcast(GameNotice.WALLET_CHANGE);
        Messenger.Broadcast(DataNotice.PLAYER_MODEL_UPDATED);
    }

    /// <summary>
    /// Поиск приобритений актива игроком
    /// </summary>
    public AssetPurchase FindAsset(IAsset asset)
    {
        switch (asset)
        {
            case Shares:
                return Assets[AssetType.Shares].Find(x => x.Asset == asset.Ticket);
            case Bond:
                return Assets[AssetType.Bonds].Find(x => x.Asset == asset.Ticket);

            default:
                return null;
        }
    }

    /// <summary>
    /// Поиск приобритений актива игроком
    /// </summary>
    public AssetPurchase FindAsset(AssetType type, IAsset asset)
    {
        return Assets[type].Find(x => x.Asset == asset.Ticket);
    }

    /// <summary>
    /// Удаление информации о приобретении актива игроком
    /// </summary>
    public void RemoveAsset(AssetType type, IAsset asset)
    {
        this.Assets[type].Remove(FindAsset(type, asset));
        Messenger.Broadcast(DataNotice.PLAYER_MODEL_UPDATED);
    }

    #region Методы прямо влияющие на данные модели

    /// <summary>
    /// Добавляет вложение в список активов игрока
    /// </summary>
    public void AddImvestment(Investment investment)
    {
        this.Investments.Add(investment);
        Messenger.Broadcast(DataNotice.PLAYER_MODEL_UPDATED);
    }

    /// <summary>
    /// Добавляет должника в список активов игрока
    /// </summary>
    public void AddDept(Dept dept)
    {
        this.Deptors.Add(dept);
        Messenger.Broadcast(DataNotice.PLAYER_MODEL_UPDATED);
    }

    /// <summary>
    /// Добавляет актив в список активов игрока
    /// </summary>
    /// <param name="assetType">Тип актива</param>
    /// <param name="asset">Актив</param>
    public void AddAsset(AssetType assetType, AssetPurchase asset)
    {
        this.Assets[assetType].Add(asset);
        Messenger.Broadcast(DataNotice.PLAYER_MODEL_UPDATED);
    }

    /// <summary>
    /// Удаляет актив со списка активов игрока
    /// </summary>
    /// <param name="assetType">Тип актива</param>
    /// <param name="asset">Актив</param>
    public void RemoveAsset(AssetType assetType, AssetPurchase asset)
    {
        this.Assets[assetType].Remove(asset);
        Messenger.Broadcast(DataNotice.PLAYER_MODEL_UPDATED);
    }

    #endregion


    /// <summary>
    /// Вычисляет общую сумму валюты, переведенные в целевую валюту
    /// </summary>
    /// <param name="currency">Целевая валюта, на которую будет происходит перевод</param>
    /// <returns>Общий размер</returns>
    public decimal CalcCurenciesValue(string currency)
    {
        decimal value = 0;

        foreach (var item in this.Wallet)
        {
            if (item.Key == currency)
                value += item.Value;
            else
                value += Controllers.CurrencyExchange.CalcSwap(item.Key, item.Value, currency, true);
        }

        return value;
    }

    /// <summary>
    /// Вычисляет сумму всех ценных бумаг имеющихся у игрока
    /// </summary>
    /// <param name="mainCurrency">Валюта, по которой будет проводиться расчет</param>
    /// <returns>Кол-во валюты / Стоимость</returns>
    public decimal CalcStocksValue(string mainCurrency)
    {
        decimal value = 0;

        foreach (var item in this.Assets[AssetType.Shares])
        {
            string currency = Controllers.StockExchange.FindStock(StockType.Shares, item.Asset).Currency;
            decimal amount = item.BuyPrices.Average(x => x) * item.Amount;

            if (currency == mainCurrency)
                value += amount;
            else
                value += Controllers.CurrencyExchange.CalcSwap(currency, amount, mainCurrency, true);
        }

        return value;
    }


    #region Триггеры событий

    private void AssetPurchaseUpdatedTrigger() => 
        Messenger.Broadcast(DataNotice.PLAYER_MODEL_UPDATED);

    private void InvestmentUpdatedTrigger() => 
        Messenger.Broadcast(DataNotice.PLAYER_MODEL_UPDATED);

    private void DeptUpdatedTrigger() =>
        Messenger.Broadcast(DataNotice.PLAYER_MODEL_UPDATED);

    private void DayChangeTrigger()
    {
        List<Investment> investments = this.Investments.ToList();
        foreach (var investment in investments)
        {
            investment.DayChangeTrigger();
        }

        List<Dept> depts = this.Deptors.ToList();
        foreach (var dept in depts)
        {
            dept.DayChangeTrigger();
        }
    }

    #endregion


    public object GetDataCopy()
    {
        return _model.Clone();
    }
}