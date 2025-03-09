using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Контроллер окна настроек
/// </summary>
public class UI_Popup_Settings : UI_Popup
{
    [Header("Раздел \"Дополнительные\"")]
    [Tooltip("Подтверждение сделок")]
    [SerializeField] private Toggle toggleConfirmTransactions;
    [Tooltip("Подтверждение вложений")]
    [SerializeField] private Toggle toggleConfirmInvestments;
    [Tooltip("Заставка появляющееся при изменении игрового дня")]
    [SerializeField] private Toggle toggleDayChangeScreensaver;
    [Tooltip("Основная валюта по которой будут проходит расчеты")]
    [SerializeField] private TMP_Dropdown dropdownMainCurrency;


    private List<string> _currencies;


    private void OnEnable()
    {
        toggleConfirmTransactions.isOn = Managers.Settings.Properties.ConfirmTransactions;
        toggleDayChangeScreensaver.isOn = Managers.Settings.Properties.DayChangeScreensaver;

        _currencies = Controllers.CurrencyExchange.GetCurrencies();

        dropdownMainCurrency.ClearOptions();
        dropdownMainCurrency.AddOptions(_currencies);
        dropdownMainCurrency.value = _currencies.IndexOf(Managers.Settings.Properties.MainCurrency);
    }


    public override void OpenPopup()
    {
        base.OpenPopup();
        Messenger<UI_Popup_Settings>.Broadcast(UINotice.SETTINGS_POPUP_OPENED, this);
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
        Messenger<UI_Popup_Settings>.Broadcast(UINotice.SETTINGS_POPUP_CLOSED, this);
    }


    /// <summary>
    /// Обработчик для переключателя "Confirm Transactions"
    /// </summary>
    public void OnConfirmTransactionsToggle()
    {
        Managers.Settings.SetConfirmTransactions(toggleConfirmTransactions.isOn);
    }

    /// <summary>
    /// Обработчик для переключателя "Day Change Screensaver"
    /// </summary>
    public void OnDayChangeScreensaverToggle()
    {
        Managers.Settings.SetDayChangeScreensaver(toggleDayChangeScreensaver.isOn);
    }

    /// <summary>
    /// Обработчик для выпадающего списка "Main Currency"
    /// </summary>
    public void OnMainCurrencyPicked()
    {
        string currency = _currencies[dropdownMainCurrency.value];
        Managers.Settings.SetMainCurrency(currency);
    }
}
