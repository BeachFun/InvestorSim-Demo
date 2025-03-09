using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Диспетчер настроек игры
/// </summary>
public class SettingsManager : MonoBehaviour, IGameManager, IDataCopable
{
    // TODO: добавить различные оповещения системы для этого менедеджера
    // TODO: вынести общие свойства менеджеров в родительский класс

    /// <summary>
    /// Статус менеджера
    /// </summary>
    public ManagerStatus status { get; private set; }

    /// <summary>
    /// Свойства настроек
    /// </summary>
    public SettingsModel Properties { get => _settings; }


    internal SettingsModel _settings;


    #region Методы запуска и Инициализации

    public IEnumerator Startup()
    {
        Messenger.Broadcast(StartupNotice.SETINGS_MANAGER_STARTING);
        status = ManagerStatus.Initializing;

        yield return null;

        // Если на момент запуска нет данных, то создаются данные по-умолчанию
        if (_settings is null) _settings = new SettingsModel();

        status = ManagerStatus.Started;
        Messenger.Broadcast(StartupNotice.SETINGS_MANAGER_STARTED);
    }


    public void UpdateData(SettingsModel settings)
    {
        _settings = settings;
    }

    #endregion


    /// <summary>
    /// Настройка свойства "Подтвеждаются ли сделки"
    /// </summary>
    public void SetConfirmTransactions(bool value)
    {
        _settings.ConfirmTransactions = value;
        Messenger.Broadcast(DataNotice.SETTINGS_MODEL_UPDATED);
    }
    /// <summary>
    /// Настройка свойства "Подтверждаются ли вклады"
    /// </summary>
    public void SetConfirmInvestments(bool value)
    {
        _settings.ConfirmInvestments = value;
        Messenger.Broadcast(DataNotice.SETTINGS_MODEL_UPDATED);
    }
    /// <summary>
    /// Настройка свойства "Открывается ли заставка смены дня"
    /// </summary>
    public void SetDayChangeScreensaver(bool value)
    {
        _settings.DayChangeScreensaver = value;
        Messenger.Broadcast(DataNotice.SETTINGS_MODEL_UPDATED);
    }

    /// <summary>
    /// Основная валюта
    /// </summary>
    /// <param name="currency"></param>
    public void SetMainCurrency(string currency)
    {
        _settings.MainCurrency = currency;
        Messenger.Broadcast(DataNotice.SETTINGS_MODEL_UPDATED);
    }

    public object GetDataCopy()
    {
        return _settings.Clone();
    }
}