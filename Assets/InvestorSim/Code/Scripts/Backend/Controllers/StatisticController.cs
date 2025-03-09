using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatisticController : MonoBehaviour, IGameController, IDataCopable
{
    public ControllerStatus status { get; private set; }


    public Dictionary<int, decimal> CapitalStory
    {
        get => _model.CapitalStory.ToDictionary(pair => pair.Key, pair => pair.Value); 
    }
    public int TotalDeals
    {
        get => _model.TotalDeals;
    }
    public int SuccesfulDelas
    {
        get => _model.SuccesfulDelas;
    }
    public int Succesfulness
    {
        get
        {
            try
            {
                return this.SuccesfulDelas / this.TotalDeals;
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }
    }
    /// <summary>
    /// Общая сумма всех средств, что имеет игрок
    /// </summary>
    public decimal OwnFunds
    {
        get => this.CurrenciesValue + this.StocksValue;
    }
    /// <summary>
    /// Общая сумма валюты, что имеет игрок
    /// </summary>
    public decimal CurrenciesValue
    {
        get => Managers.Player.CalcCurenciesValue(Managers.Settings.Properties.MainCurrency);
    }
    /// <summary>
    /// Общая сумма всех ценных бумаг, что имеет игрок
    /// </summary>
    public decimal StocksValue
    {
        get => Managers.Player.CalcStocksValue(Managers.Settings.Properties.MainCurrency);
    }
    public float BestSuccesfulness
    {
        get => _model.BestSuccesfulness;
    }


    private Statistic _model;


    #region Методы запуска и Инициализации

    private void OnDestroy()
    {
        if (status == ControllerStatus.Started)
        {
            Messenger.RemoveListener(GameNotice.DAY_CHANGE, CollectData);
            Messenger.RemoveListener(GameNotice.WALLET_CHANGE, CollectData);
        }
    }

    public IEnumerator Startup()
    {
        status = ControllerStatus.Initializing;

        yield return null;

        // Если на момент запуска нет данных, то создаются данные по-умолчанию
        if (_model is null) _model = new Statistic();

        Messenger.AddListener(GameNotice.DAY_CHANGE, CollectData);
        Messenger.AddListener(GameNotice.WALLET_CHANGE, CollectData);

        status = ControllerStatus.Started;
        Debug.Log("Statistic Controller started");
    }

    public void UpdateData(Statistic data)
    {
        _model = data;
    }

    #endregion


    private void CollectData()
    {
        this._model.CapitalStory[Managers.Game.DaysInGame] = this.OwnFunds;
        Messenger.Broadcast(DataNotice.MODEL_UPDATED);
    }


    public object GetDataCopy()
    {
        return _model.Clone();
    }
}