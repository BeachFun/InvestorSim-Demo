using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Контроллер управляющий корпорациями игры, представляющие из себя игровые объекты
/// </summary>
public class CorporationsController : MonoBehaviour, IGameController, IDataCopable
{
    #region Поля Unity

    [Header("Настройка корпораций")]
    [Tooltip("Шанс дивидендной выплаты | 1..100")]
    [Range(1f, 100f)] public float dividendAnnounceChance = 50f;
    [Tooltip("Шанс успешного финансового отчета | 1..100")]
    [Range(1f, 100f)] public float financeStatementChance = 50f;

    [Header("Настройка дивиденых выплат")]
    [Tooltip("Нижняя граница выплат дивидендов в % от стоимости акций")]
    [Range(.1f, 1f)] public float dinidendLowBorder = 0.1f;
    [Tooltip("Верхняя граница выплат дивидендов в % от стоимости акций")]
    [Range(2f, 10f)] public float dinidendHighBorder = 2f;

    #endregion


    public ControllerStatus status { get; private set; }

    /// <summary>
    /// Выдает список корпораций
    /// </summary>
    public List<Corporation> List
    {
        get => _corporations;
    }

    internal bool IsStarted { get; private set; } // не сохранять


    private List<Corporation> _corporations;
    private List<Shares> _shares; // не сохранять
    private List<Bond> _bonds; // не сохранять
    private bool _dataCreated; // не сохранять

    #region Методы запуска и Инициализации

    private void OnDestroy()
    {
        if (status == ControllerStatus.Started)
        {
            if (_dataCreated)
                Messenger.RemoveListener(StartupNotice.ALL_CONTROLLERS_STARTED, LinkedData);

            Messenger.RemoveListener(GameNotice.DAY_CHANGE, DayChangeTrigger);
        }
    }

    public IEnumerator Startup()
    {
        Debug.Log("Corporations controller starting...");
        status = ControllerStatus.Initializing;

        yield return null;


        // Если на момент запуска нет данных, то создаются данные по-умолчанию
        if (_corporations is null)
        {
            (List<Corporation>, List<Shares>, List<Bond>) corpInfo = GameTemplates.GetCorporationsInfo1();
            _shares = corpInfo.Item2;
            _bonds = corpInfo.Item3;
            _corporations = corpInfo.Item1;

            _dataCreated = true;
            Messenger.AddListener(StartupNotice.ALL_CONTROLLERS_STARTED, LinkedData);
        }

        Messenger.AddListener(GameNotice.DAY_CHANGE, DayChangeTrigger);

        status = ControllerStatus.Started;
        Debug.Log("Corporations controller started...");
    }

    /// <summary>
    /// Связывание данных с биржей и асетами
    /// </summary>
    private void LinkedData()
    {
        // Заполнение модели AssetsModel
        foreach (var shares in _shares)
            Controllers.Assets.AddAsset(shares);
        foreach (var bonds in _bonds)
            Controllers.Assets.AddAsset(bonds);

        // Листинг акций/облигаций компаний на бирже
        //Debug.Log(_corporations.Count);
        //int i = 1;
        foreach (var corp in _corporations)
        {
            //Debug.Log($"{i}) {corp.Name}");
            if (corp.SharesOrdTicket is not null)
            {
                if (Controllers.StockExchange.FindStock(StockType.Shares, corp.SharesOrdTicket) is null)
                    Controllers.StockExchange.AddNewStock(StockType.Shares, corp.SharesOrdTicket);
            }
            if (corp.SharesPrivTicket is not null)
            {
                if (Controllers.StockExchange.FindStock(StockType.Shares, corp.SharesPrivTicket) is null)
                    Controllers.StockExchange.AddNewStock(StockType.Shares, corp.SharesPrivTicket);
            }
            foreach (var bonds in corp.Bonds)
            {
                if (Controllers.StockExchange.FindStock(StockType.Bonds, bonds) is null)
                    Controllers.StockExchange.AddNewStock(StockType.Bonds, bonds);
            }
            //i++;
        }

        _shares = null;
        _bonds = null;

        IsStarted = true;
    }

    public void UpdateData(List<Corporation> data)
    {
        _corporations = data;
    }

    #endregion


    /// <summary>
    /// Добавляет новую корпорацию в список корпораций
    /// </summary>
    /// <param name="corporation">Объект корпорации</param>
    public void AddCorporation(Corporation corporation)
    {
        _corporations.Add(corporation);
        Messenger.Broadcast(DataNotice.CORPORATIONS_MODEL_UPDATED);
    }

    /// <summary>
    /// Поиск компании по названию
    /// </summary>
    /// <param name="corpName">Название компании</param>
    public Corporation FindCorporation(string corpName)
    {
        return _corporations.First(x => x.Name == corpName);
    }


    private void DayChangeTrigger()
    {
        Messenger.Broadcast(DataNotice.CORPORATIONS_MODEL_UPDATED);
    }


    public object GetDataCopy()
    {
        return _corporations.Select(x => x.Clone() as Corporation).ToList();
    }
}