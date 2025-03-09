using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CurrencyEchangeController : MonoBehaviour, IGameController, IDataCopable
{
    // TODO: Исправить зависимость свойств валют

    // ставка комиссии за операцию / в частности это просто величина потери при обмене
    [SerializeField] private decimal comissionRate = (decimal)0.01; // 1%
    [Tooltip("Минимальное кол-во валют для обмена")]
    [SerializeField] private int minSizeSwap = 10;

    public ControllerStatus status { get; private set; }

    public Dictionary<string, double> Currencies { get => _model.Currencies; }
    public string USD { get => "$"; }
    public string RUB { get => "₽"; }
    public string EUR { get => "€"; }


    private decimal tolerance = 0.0001M;
    private CurrencyExchange _model;


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
        if (_model is null) _model = new CurrencyExchange();

        Messenger.AddListener(GameNotice.DAY_CHANGE, DayChangeTrigger);

        status = ControllerStatus.Started;
        Messenger.Broadcast(StartupNotice.CURRENCY_CONTROLLER_STARTED);
    }

    public void UpdateData(CurrencyExchange data)
    {
        _model = data;
    }

    #endregion


    /// <summary>
    /// Рассчитывает получаемое кол-во конвертируемых денег
    /// </summary>
    /// <returns>Кол-во валюты</returns>
    public decimal CalcSwap(string currency1, decimal amount1, string currency2)
    {
        decimal c1Rate = (decimal)_model.Currencies[currency1];
        decimal c2Rate = (decimal)_model.Currencies[currency2];

        decimal amount2 = c2Rate / c1Rate * amount1;

        return amount2 * (1 - comissionRate);
    }
    /// <summary>
    /// Рассчитывает получаемое кол-во конвертируемых денег
    /// </summary>
    /// <param name="comission">Учитывать комиссию?</param>
    /// <returns>Кол-во валюты</returns>
    public decimal CalcSwap(string currency1, decimal amount1, string currency2, bool comission)
    {
        decimal c1Rate = (decimal)_model.Currencies[currency1];
        decimal c2Rate = (decimal)_model.Currencies[currency2];

        decimal amount2 = c2Rate / c1Rate * amount1;

        if (comission)
            return amount2 * (1 - comissionRate);
        else
            return amount2;
    }


    /// <summary>
    /// Рассчитывает кол-во комиссия для обмена указанного кол-ва валюты | Комиссия от конечной валюты
    /// </summary>
    /// <param name="currency1"></param>
    /// <param name="amount1"></param>
    /// <param name="currency2"></param>
    /// <returns></returns>
    public decimal CalcComission(string currency1, decimal amount1, string currency2)
    {
        decimal c1Rate = (decimal)_model.Currencies[currency1];
        decimal c2Rate = (decimal)_model.Currencies[currency2];

        decimal amount2 = c2Rate / c1Rate * amount1;

        return amount2 * (comissionRate);
    }

    /// <summary>
    /// Обмен валюты на другую валюту
    /// </summary>
    /// <param name="currency1">Валюта обмена</param>
    /// <param name="amount1">Кол-во currency1 для обмена</param>
    /// <param name="currency2">Валюта получения</param>
    public void Swap(string currency1, decimal amount1, string currency2)
    {
        // TODO: Добавить оповещения для системы оповещений (Messenger)

        if (!_model.Currencies.Keys.Contains(currency1) || !_model.Currencies.Keys.Contains(currency2))
            throw new Exception("Такой валюты нет в списках валют обменника");

        if (1 - amount1 / Managers.Player.Wallet[currency1] < tolerance)
        {
            // значения близки друг к другу
            Debug.Log("приравнивание значений | значения близки друг к другу");
            amount1 = Managers.Player.Wallet[currency1];
        }

        if (Managers.Player.Wallet[currency1] < amount1)
        {
            // Messenger.Broadcast(GameNotice.LOW_MONEY_IN_WALLET); // TODO: не работает
            Managers.UI.ShowLowMoneyNotice();
            Debug.LogError("У игрока недостаточное кол-во валюты для обмена");
            return;
        }

        string ops = string.Format("Обмен {0} на {1}", currency1, currency2);
        try
        {
            ops += "\n- начало обмена";
            ops += "\n- вычисление взимаемой комиссии";
            decimal comission = this.CalcComission(currency1, amount1, currency2);
            ops += "\n- вычисление кол-ва валюты для получения";
            decimal amount2 = this.CalcSwap(currency1, amount1, currency2);
            ops += "\n- успешное вычисление";
            ops += "\n- перевод средств";
            Managers.Player.ChangeWalletMoney(currency1, -amount1);
            Managers.Player.ChangeWalletMoney(currency2, amount2);
            ops += "\n- успешный обмен валюты";
            ops += "\n- добавление транзации в историю";
            Controllers.Transactions.AddTransaction(new Transaction()
            {
                Type = TransactionType.Swap,
                ItemId = currency2,
                Date = Managers.Game.Date,
                Price = amount1,
                Currency = currency1,
                Quantity = amount2,
                Comission = comission
            });
            ops += "\n- транзакция успешно добавлена";
            ops += "\n- завершение операции";
        }
        catch
        {
            ops += "\n- ошибка обмена";
        }

        Debug.Log(ops);
    }

    /// <summary>
    /// Возвщарает список валют на которую можно обменять указанную валюту
    /// </summary>
    public List<string> GetSwapList(string currency)
    {
        return this.Currencies.Keys.Where(x => x != currency).ToList();
    }

    /// <summary>
    /// Возвщает минимальное значение кол-ва валюта для обмена 
    /// </summary>
    public decimal GetMinValueSwap(string currency1, string currency2)
    {
        return this.GetCourse(currency1, currency2) * minSizeSwap;
    }


    /// <summary>
    /// Возвщает курс обмена. Кол-во Currency1 для покупки 1-ой единицы Currency2
    /// </summary>
    public decimal GetCourse(string currency1, string currency2)
    {
        return (decimal)this.Currencies[currency1] / (decimal)this.Currencies[currency2];
    }


    /// <summary>
    /// Возвращает список всех валют в игре
    /// </summary>
    /// <returns></returns>
    public List<string> GetCurrencies()
    {
        // TODO: переделать в свойство
        return new List<string>() { this.RUB, this.USD, this.EUR };
    }


    private void DayChangeTrigger()
    {
        //_model.Currencies[CurrencyFilter.USD] = UnityEngine.Random.Range((float)0.98, (float)1.04);
        //_model.Currencies[CurrencyFilter.RUB] = UnityEngine.Random.Range((float)60.0, (float)80.0);
        //_model.Currencies[CurrencyFilter.USD] = UnityEngine.Random.Range((float)0.8, (float)1.01);

        Messenger.Broadcast(DataNotice.CURRENCY_EXCHANGE_UPDATED);
    }


    public object GetDataCopy()
    {
        return _model.Clone();
    }
}
