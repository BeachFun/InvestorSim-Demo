using UnityEngine;
using TMPro;

/// <summary>
/// —одержит ссылки на элементы всплывающего окна валют
/// </summary>
public class UI_Popup_Currency : UI_Popup
{
    [Tooltip("—сылка на индикатор кол-ва рублей у игрока")]
    [SerializeField] private TMP_Text RUB;
    [Tooltip("—сылка на индикатор кол-ва долларов у игрока")]
    [SerializeField] private TMP_Text USD;
    [Tooltip("—сылка на индикатор кол-ва евро у игрока")]
    [SerializeField] private TMP_Text EUR;


    private void Awake()
    {
        Messenger.AddListener(GameNotice.WALLET_CHANGE, UpdateView);
    }

    private void OnEnable()
    {
        UpdateView();
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameNotice.WALLET_CHANGE, UpdateView);
    }


    public override void OpenPopup()
    {
        base.OpenPopup();
        Messenger<UI_Popup_Currency>.Broadcast(UINotice.CURRENCY_EXCHANGE_POPUP_OPENED, this);
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
        Messenger<UI_Popup_Currency>.Broadcast(UINotice.CURRENCY_EXCHANGE_POPUP_CLOSED, this);
    }


    private void UpdateView()
    {
        RUB.text = string.Format("{0} {1}", Managers.Player.Wallet[Controllers.CurrencyExchange.RUB].AddNumSpaces(), Controllers.CurrencyExchange.RUB);
        USD.text = string.Format("{0} {1}", Managers.Player.Wallet[Controllers.CurrencyExchange.USD].AddNumSpaces(), Controllers.CurrencyExchange.USD);
        EUR.text = string.Format("{0} {1}", Managers.Player.Wallet[Controllers.CurrencyExchange.EUR].AddNumSpaces(), Controllers.CurrencyExchange.EUR);
    }
}