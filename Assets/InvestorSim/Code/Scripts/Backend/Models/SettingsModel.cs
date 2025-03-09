using System;

[Serializable]

/// <summary>
/// Модель по MVC содержащая настройки игры
/// </summary>
public class SettingsModel : IGameModel
{
    public SettingsModel()
    {
        ConfirmTransactions = true;
        ConfirmInvestments = true;
        DayChangeScreensaver = true;

        Messenger.AddListener(StartupNotice.CURRENCY_CONTROLLER_STARTED, this.CurrencyExchangeStartedTrigger);
    }

    /// <summary>
    /// Подтвеждаются ли сделки
    /// </summary>
    public bool ConfirmTransactions { get; set; }
    /// <summary>
    /// Подтверждаются ли вклады
    /// </summary>
    public bool ConfirmInvestments { get; set; }
    /// <summary>
    /// Открывается ли заставка смены дня
    /// </summary>
    public bool DayChangeScreensaver { get; set; }
    /// <summary>
    /// Основная валюта, по которой проводятся вычисления
    /// </summary>
    public string MainCurrency { get; set; }

    private void CurrencyExchangeStartedTrigger()
    {
        MainCurrency = Controllers.CurrencyExchange.RUB;
    }

    public object Clone()
    {
        return new SettingsModel()
        {
            ConfirmInvestments = this.ConfirmInvestments,
            ConfirmTransactions = this.ConfirmTransactions,
            DayChangeScreensaver = this.DayChangeScreensaver,
            MainCurrency = this.MainCurrency
        };
    }
}