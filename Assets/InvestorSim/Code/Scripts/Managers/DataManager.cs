using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

[RequireComponent(typeof(DataSerializer))]

/// <summary>
/// Отвечает за сохранение и загрузку состояния экономической системы в процессе игры. Также за создание первичной экономической структуры
/// </summary>
public class DataManager : MonoBehaviour, IGameManager
{
    /* Класс сам загружает и сохраняет данные менеджеров и контроллеров.
     * Если данных нет, то они сами создают себе данные по-умолчанию.
     */


    [Tooltip("Название файла в котором будут храниться данные (data.xml)")]
    [SerializeField] private string fileName = "data.gd";


    /// <summary>
    /// Статус диспетчера
    /// </summary>
    public ManagerStatus status { get; private set; }

    /// <summary>
    /// Модуль загрузки/сохранения
    /// </summary>
    internal DataSerializer Saver
    {
        get => _dataSaver;
    }


    private DataSerializer _dataSaver;
    // Ключи к данным
    private string _gmId = "GM1";
    private string _pmId = "PM1";
    private string _smId = "SM1";
    private string _tmId = "TM1";
    private string _cmId = "CM1";
    private string _amId = "AM1";
    private string _nmId = "NM1";
    private string _omId = "OM1";
    private string _semId = "SEM1";
    private string _cemId = "CEM1";
    private string _stmId = "STM1";


    #region Методы запуска и инициализации

    private void OnDestroy()
    {
        if (status == ManagerStatus.Started)
        {
            // Отписка на различные события для автосохранения
            Messenger.RemoveListener(DataNotice.GAME_MODEL_UPDATED, this.SaveGameData);
            Messenger.RemoveListener(DataNotice.PLAYER_MODEL_UPDATED, this.SavePlayerData);
            Messenger.RemoveListener(DataNotice.SETTINGS_MODEL_UPDATED, this.SaveSettingsData);
            Messenger.RemoveListener(DataNotice.OFFER_MODEL_UPDATED, this.SaveOfferModel);
            Messenger.RemoveListener(DataNotice.NEWS_MODEL_UPDATED, this.SaveNewsModel);
            Messenger.RemoveListener(DataNotice.TRANSACTION_MODEL_UPDATED, this.SaveTransactionModel);
            Messenger.RemoveListener(DataNotice.CORPORATIONS_MODEL_UPDATED, this.SaveCorpModel);
            Messenger.RemoveListener(DataNotice.ASSETS_MODEL_UPDATED, this.SaveAssetsModel);
            Messenger.RemoveListener(DataNotice.STOCK_EXCHANGE_UPDATED, this.SaveStockExchangeData);
            Messenger.RemoveListener(DataNotice.CURRENCY_EXCHANGE_UPDATED, this.SaveCurrencyExchangeData);
            Messenger.RemoveListener(DataNotice.MODEL_UPDATED, this.SaveStatisticData);
            // Отписка на различные события
            Messenger.RemoveListener(GameNotice.RESTART, this.DeleteAllData);
        }
    }

    /// <summary>
    /// Запуск контроллера данными игры
    /// </summary>
    public IEnumerator Startup()
    {
        Messenger.Broadcast(StartupNotice.DATA_MANAGER_STARTING);
        status = ManagerStatus.Initializing;

        _dataSaver = GetComponent<DataSerializer>();
        _dataSaver.DataPath = Path.Combine(Application.persistentDataPath, fileName);

        this.LoadData();

        // Подписка на различные события для автосохранения
        Messenger.AddListener(DataNotice.GAME_MODEL_UPDATED, this.SaveGameData);
        Messenger.AddListener(DataNotice.PLAYER_MODEL_UPDATED, this.SavePlayerData);
        Messenger.AddListener(DataNotice.SETTINGS_MODEL_UPDATED, this.SaveSettingsData);
        Messenger.AddListener(DataNotice.OFFER_MODEL_UPDATED, this.SaveOfferModel);
        Messenger.AddListener(DataNotice.NEWS_MODEL_UPDATED, this.SaveNewsModel);
        Messenger.AddListener(DataNotice.TRANSACTION_MODEL_UPDATED, this.SaveTransactionModel);
        Messenger.AddListener(DataNotice.CORPORATIONS_MODEL_UPDATED, this.SaveCorpModel);
        Messenger.AddListener(DataNotice.ASSETS_MODEL_UPDATED, this.SaveAssetsModel);
        Messenger.AddListener(DataNotice.STOCK_EXCHANGE_UPDATED, this.SaveStockExchangeData);
        Messenger.AddListener(DataNotice.CURRENCY_EXCHANGE_UPDATED, this.SaveCurrencyExchangeData);
        Messenger.AddListener(DataNotice.MODEL_UPDATED, this.SaveStatisticData);
        // Подписка на различные события
        Messenger.AddListener(GameNotice.RESTART, this.DeleteAllData);

        status = ManagerStatus.Started;
        Messenger.Broadcast(StartupNotice.DATA_MANAGER_STARTED);

        yield return null;
    }

    #endregion


    #region Методы загрузки данных

    /// <summary>
    /// Загрузка данных менеджеров и контроллеров
    /// </summary>
    private void LoadData()
    {
        // Менеджеры
        this.LoadGameData();
        this.LoadPlayerData();
        this.LoadSettingsData();
        // Контроллеры
        this.LoadTransactionModel();
        this.LoadCorpModel();
        this.LoadAssetsModel();
        this.LoadNewsModel();
        this.LoadOfferModel();
        this.LoadStockExchangeData();
        this.LoadCurrencyExchangeData();
        this.LoadStatisticData();
    }

    private void LoadGameData()
    {
        var model = this.Saver.Load<GameModel>(_gmId);
        Managers.Game.UpdateData(model);
    }
    private void LoadPlayerData()
    {
        var model = this.Saver.Load<PlayerModel>(_pmId);
        Managers.Player.UpdateData(model);
    }
    private void LoadSettingsData()
    {
        var model = this.Saver.Load<SettingsModel>(_smId);
        Managers.Settings.UpdateData(model);
    }

    private void LoadTransactionModel()
    {
        var model = this.Saver.Load<TransactionModel>(_tmId);
        Controllers.Transactions.UpdateData(model);
    }
    private void LoadCorpModel()
    {
        var model = this.Saver.Load<List<Corporation>>(_cmId);
        Controllers.Corporations.UpdateData(model);
    }
    private void LoadAssetsModel()
    {
        var modelData = this.Saver.Load<Dictionary<object, List<(decimal, DateTime)>>>(_amId);
        if (modelData is null) return;

        AssetsModel model = new AssetsModel(modelData);

        Controllers.Assets.UpdateData(model);
    }
    private void LoadNewsModel()
    {
        var model = this.Saver.Load<NewsModel>(_nmId);
        Controllers.News.UpdateData(model);
    }
    private void LoadOfferModel()
    {
        var data = this.Saver.Load<List<Dictionary<string, object>>>(_omId);
        if (data is null) return;

        var list = new List<Offer>();
        foreach(var item in data) list.Add(new Offer(item));

        var model = new OfferModel()
        {
            OfferList = list
        };

        Controllers.Offers.UpdateData(model);
    }
    private void LoadStockExchangeData()
    {
        var model = this.Saver.Load<StockExchange>(_semId);
        Controllers.StockExchange.UpdateData(model);
    }
    private void LoadCurrencyExchangeData()
    {
        var model = this.Saver.Load<CurrencyExchange>(_cemId);
        Controllers.CurrencyExchange.UpdateData(model);
    }
    private void LoadStatisticData()
    {
        var model = this.Saver.Load<Statistic>(_stmId);
        Controllers.Statistic.UpdateData(model);
    }

    #endregion

    #region Методы сохранения данных

    //private void OnApplicationPause(bool pause)
    //{
    //    SaveAllData();
    //}

    //private void OnApplicationQuit()
    //{
    //    SaveAllData();
    //}

    private void SaveAllData()
    {
        this.SaveGameData();
        this.SavePlayerData();
        this.SaveSettingsData();

        this.SaveTransactionModel();
        this.SaveCorpModel();
        this.SaveAssetsModel();
        this.SaveNewsModel();
        this.SaveOfferModel();
        this.SaveStockExchangeData();
        this.SaveCurrencyExchangeData();
        this.SaveStatisticData();
    }

    private void SaveGameData()
    {
        this.Saver.Dump(Managers.Game.GetDataCopy() as GameModel, _gmId);
    }
    private void SavePlayerData()
    {
        this.Saver.Dump(Managers.Player.GetDataCopy() as PlayerModel, _pmId);
    }
    private void SaveSettingsData()
    {
        this.Saver.Dump(Managers.Settings.GetDataCopy() as SettingsModel, _smId);
    }

    private void SaveTransactionModel()
    {
        this.Saver.Dump(Controllers.Transactions.GetDataCopy() as TransactionModel, _tmId);
    }
    private void SaveCorpModel()
    {
        this.Saver.Dump(Controllers.Corporations.GetDataCopy() as List<Corporation>, _cmId);
    }
    private void SaveAssetsModel()
    {
        this.Saver.Dump(Controllers.Assets.GetDataCopy() as Dictionary<object, List<(decimal, DateTime)>>, _amId);
    }
    private void SaveNewsModel()
    {
        this.Saver.Dump(Controllers.News.GetDataCopy() as NewsModel, _nmId);
    }
    private void SaveOfferModel()
    {
        this.Saver.Dump(Controllers.Offers.GetDataCopy() as List<Dictionary<string, object>>, _omId);
    }
    private void SaveStockExchangeData()
    {
        this.Saver.Dump(Controllers.StockExchange.GetDataCopy() as StockExchange, _semId);
    }
    private void SaveCurrencyExchangeData()
    {
        this.Saver.Dump(Controllers.CurrencyExchange.GetDataCopy() as CurrencyExchange, _cemId);
    }
    private void SaveStatisticData()
    {
        this.Saver.Dump(Controllers.Statistic.GetDataCopy() as Statistic, _stmId);
    }

    #endregion


    private void DeleteAllData()
    {
        var model = this.Saver.Load<SettingsModel>(_smId);
        this.Saver.Save(model, _smId);
    }
}