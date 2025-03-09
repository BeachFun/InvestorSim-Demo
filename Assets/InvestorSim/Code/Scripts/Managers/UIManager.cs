using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IGameManager
{
    [Header("Навигация по главным страницам")]
    [SerializeField] private UI_TabGroup tabGroup;
    [Header("Дополнительные страницы")]
    [SerializeField] private UI_Page_Story storyPage;
    [Header("Верхняя панель с индикаторами")]
    [SerializeField] private GameObject RUB;
    [SerializeField] private GameObject USD;
    [SerializeField] private GameObject EUR;
    [SerializeField] private TextMeshProUGUI Date;
    [Header("Остальная")]
    [SerializeField] private TextMeshProUGUI DaysInGame;
    [Header("Высплывающие окна")]
    [Tooltip("Панель на которой будут генерироваться Popups")]
    [SerializeField] private GameObject popupCanvas;
    [SerializeField] private UI_Popup_Share sharePopup;
    [SerializeField] private UI_Popup_Bond bondPopup;
    [SerializeField] private UI_Popup_Company companyPopup;
    [SerializeField] private UI_Popup_Offer offerPopup;
    [SerializeField] private UI_Popup_News newsPopup;
    [SerializeField] private UI_Popup_Investment investmentPopup;
    [SerializeField] private UI_Popup_Currency currencyExchangePopup;
    [SerializeField] private GameObject lowMoneyPopup;
    [SerializeField] private STController tooltip;
    [Header("Диалоговые окна")]
    [SerializeField] private GameObject offerDialog;
    [SerializeField] private GameObject confirmDialog;
    [Header("Отметки оповещений")]
    [SerializeField] private Image newNewsMark;
    [SerializeField] private Image newOfferMark;
    [Header("Другие холсты")]
    [Tooltip("Заставка обновления дня")]
    [SerializeField] private GameObject dayChangeScreenSaver;
    [Header("Настройки отметок оповещений")]
    [Tooltip("Цвет отметки при слабой новости")]
    [SerializeField] public Color lowPower = Color.green;
    [Tooltip("Цвет отметки при новости средней силы")]
    [SerializeField] public Color mediumPower = Color.yellow;
    [Tooltip("Цвет отметки при сильной новости")]
    [SerializeField] public Color highPower = Color.red;
    [Tooltip("Цвет всех отметок")]
    [SerializeField] public Color color = Color.red;


    public ManagerStatus status { get; private set; }
    public STController Tooltip
    {
        get => tooltip;
    }


    private UI_Popup _prevPopup;
    private UI_Popup _currPopup;


    #region Методы запуска и Инициализации

    private void Awake()
    {
        newOfferMark.color = color;

        storyPage.gameObject.SetActive(true);
        dayChangeScreenSaver.SetActive(true);
        tooltip.gameObject.SetActive(true);
    }

    private void Start()
    {
        storyPage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (status == ManagerStatus.Started)
        {
            // Отписка на всякие события
            Messenger.RemoveListener(GameNotice.DAY_CHANGE, this.DayChangeTrigger);
            Messenger.RemoveListener(GameNotice.WALLET_CHANGE, this.WalletChangeTrigger);
            Messenger.RemoveListener(StartupNotice.ALL_MANAGERS_STARTED, this.GameAwakedTrigger);
            Messenger.RemoveListener(StartupNotice.ALL_CONTROLLERS_STARTED, this.AllControllersStartedTrigger);
            Messenger.RemoveListener(GameNotice.LOW_MONEY_IN_WALLET, this.ShowLowMoneyNotice);
            // Отписка от событий связанных со всплывающими окнами
            Messenger<UI_Popup>.RemoveListener(UINotice.SHARE_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.BOND_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.OFFER_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.NEWS_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.INVESTMENT_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.CURRENCY_EXCHANGE_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.SETTINGS_POPUP_OPENED, this.PopupOpenedTrigger);
            // Отписка от событий закрытия всплывающих окон
            Messenger<UI_Popup>.RemoveListener(UINotice.SHARE_POPUP_CLOSED, this.PopupClosedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.BOND_POPUP_CLOSED, this.PopupClosedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.OFFER_POPUP_CLOSED, this.PopupClosedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.NEWS_POPUP_CLOSED, this.PopupClosedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.INVESTMENT_POPUP_CLOSED, this.PopupClosedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.CURRENCY_EXCHANGE_POPUP_CLOSED, this.PopupClosedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.SETTINGS_POPUP_CLOSED, this.PopupClosedTrigger);

        }
    }

    public IEnumerator Startup()
    {
        Messenger.Broadcast(StartupNotice.UI_MANAGER_STARTING);
        status = ManagerStatus.Initializing;

        yield return null;

        // Подписка на всякие события
        Messenger.AddListener(GameNotice.DAY_CHANGE, this.DayChangeTrigger);
        Messenger.AddListener(GameNotice.WALLET_CHANGE, this.WalletChangeTrigger);
        Messenger.AddListener(StartupNotice.ALL_MANAGERS_STARTED, this.GameAwakedTrigger);
        Messenger.AddListener(StartupNotice.ALL_CONTROLLERS_STARTED, this.AllControllersStartedTrigger);
        Messenger.AddListener(GameNotice.LOW_MONEY_IN_WALLET, this.ShowLowMoneyNotice);
        // Подписка на события открытия всплывающих окон
        Messenger<UI_Popup>.AddListener(UINotice.SHARE_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.BOND_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.OFFER_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.NEWS_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.INVESTMENT_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.CURRENCY_EXCHANGE_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.SETTINGS_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.COMPANY_POPUP_OPENED, this.PopupOpenedTrigger);
        // Подписка на события закрытия всплывающих окон
        Messenger<UI_Popup>.AddListener(UINotice.SHARE_POPUP_CLOSED, this.PopupClosedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.BOND_POPUP_CLOSED, this.PopupClosedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.OFFER_POPUP_CLOSED, this.PopupClosedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.NEWS_POPUP_CLOSED, this.PopupClosedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.INVESTMENT_POPUP_CLOSED, this.PopupClosedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.CURRENCY_EXCHANGE_POPUP_CLOSED, this.PopupClosedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.SETTINGS_POPUP_CLOSED, this.PopupClosedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.COMPANY_POPUP_CLOSED, this.PopupClosedTrigger);


        status = ManagerStatus.Started;
        Messenger.Broadcast(StartupNotice.UI_MANAGER_STARTED);
    }

    #endregion


    #region Триггеры событий

    private void DayChangeTrigger()
    {
        try
        {
            Date.text = string.Format("{0}.{1}\n{2}", Managers.Game.Date.Day, Managers.Game.Date.Month, Managers.Game.Date.Year); ;
            DaysInGame.text = string.Format("{0}", Managers.Game.DaysInGame);

            if (Controllers.Offers.NewOffers.Count > 0)
                newOfferMark.gameObject.SetActive(true);
            else
                newOfferMark.gameObject.SetActive(false);

            List<News> newsList = Controllers.News.ActuallyNews.Where(x => x.ReleaseDate == Managers.Game.Date).ToList();
            if (newsList.Count > 0)
            {
                newNewsMark.gameObject.SetActive(true);

                NewsPower newsPower = NewsPower.Unknown;
                foreach (News news in newsList)
                {
                    if ((int)newsPower < (int)news.Power)
                    {
                        newsPower = news.Power;
                        newNewsMark.color = newsPower switch
                        {
                            NewsPower.Low => lowPower,
                            NewsPower.Medium => mediumPower,
                            NewsPower.High => highPower,
                            _ => color
                        };
                    }
                }
            }
            else newNewsMark.gameObject.SetActive(false);
        }
        catch { }

        //newNewsMark.gameObject.tag
    }

    private void WalletChangeTrigger()
    {
        RUB.SetActive(false);
        USD.SetActive(false);
        EUR.SetActive(false);

        List<(string, decimal)> wallet = Managers.Player.Wallet.Select(x => (x.Key, x.Value)).ToList();
        wallet = wallet.OrderByDescending(x => x.Item2).ToList();

        if (wallet[0].Item1 == Controllers.CurrencyExchange.RUB) RUB.SetActive(true);
        if (wallet[0].Item1 == Controllers.CurrencyExchange.USD) USD.SetActive(true);
        if (wallet[0].Item1 == Controllers.CurrencyExchange.EUR) EUR.SetActive(true);

        if (wallet[1].Item1 == Controllers.CurrencyExchange.RUB) RUB.SetActive(true);
        if (wallet[1].Item1 == Controllers.CurrencyExchange.USD) USD.SetActive(true);
        if (wallet[1].Item1 == Controllers.CurrencyExchange.EUR) EUR.SetActive(true);

        RUB.GetComponentInChildren<TMP_Text>().text = string.Format("{0}", SharpHelper.AddNumSpaces(Managers.Player.Wallet[Controllers.CurrencyExchange.RUB]));
        USD.GetComponentInChildren<TMP_Text>().text = string.Format("{0}", SharpHelper.AddNumSpaces(Managers.Player.Wallet[Controllers.CurrencyExchange.USD]));
        EUR.GetComponentInChildren<TMP_Text>().text = string.Format("{0}", SharpHelper.AddNumSpaces(Managers.Player.Wallet[Controllers.CurrencyExchange.EUR]));
    }

    private void GameAwakedTrigger()
    {
        if (tabGroup != null)
            tabGroup.SwitchTab(2);
    }

    private void AllControllersStartedTrigger()
    {
        WalletChangeTrigger();
        DayChangeTrigger();
    }

    private void PopupOpenedTrigger(UI_Popup popup)
    {
        if (_currPopup is not null) _currPopup.ClosePopup();
        _currPopup = popup;

        //Debug.Log($"1. {_currPopup is not null} \n2. {_prevPopup is not null}");
    }

    private void PopupClosedTrigger(UI_Popup popup)
    {
        if (_prevPopup is not null && _currPopup is not null)
        {
            if (_prevPopup is UI_Popup_News)
            {
                //Debug.Log("Открытие окна новости"); // TODO: доработать
                //_prevPopup.OpenPopup();
            }
        }

        _prevPopup = _currPopup;
        _currPopup = null;

        //Debug.Log($"1. {_currPopup is not null} \n2. {_prevPopup is not null}");
    }

    #endregion


    public void NewNewsMarkActivate()
    {
        // this
    }



    /// <summary>
    /// Открывает всплывающее окно с данными о нажатой карточки
    /// </summary>
    public void OpenPopup(ICard card)
    {
        switch (card)
        {
            case UI_ShareCard:
                OpenSharePopup((card as UI_ShareCard).Shares);
                break;
            case UI_Assets_ShareCard:
                OpenSharePopup((card as UI_Assets_ShareCard).Shares);
                break;
            case UI_BondCard:
                OpenBondsPopup((card as UI_BondCard).Bonds);
                break;
            case UI_Assets_BondCard:
                OpenBondsPopup((card as UI_Assets_BondCard).Bonds);
                break;
            case UI_OfferCard:
                OpenOfferPopup((card as UI_OfferCard).Offer);
                break;
            case UI_InvestmentCard:
                OpenInvestmentPopup((card as UI_InvestmentCard).Investment);
                break;
        }
    }

    /// <summary>
    /// Открывает высплывающее окно предложения
    /// </summary>
    /// <param name="offer">Предложение</param>
    public void OpenOfferPopup(Offer offer)
    {
        //Instantiate(popupCanvas, offerPopup.transform);
        offerPopup.UpdateData(offer);
        offerPopup.OpenPopup();
    }

    /// <summary>
    /// Открывает всплывающее окно вклада
    /// </summary>
    /// <param name="investment">Вложение в пирамиду</param>
    public void OpenInvestmentPopup(Investment investment)
    {
        investmentPopup.UpdateData(investment);
        investmentPopup.OpenPopup();
    }

    /// <summary>
    /// Открывает всплывающее окна валюты игрока
    /// </summary>
    public void OpenCurrencyExchPopup()
    {
        currencyExchangePopup.OpenPopup();
    }

    /// <summary>
    /// Открывает всплывающее окно акции
    /// </summary>
    /// <param name="shares">Акция</param>
    public void OpenSharePopup(Shares shares)
    {
        sharePopup.Shares = shares;
        sharePopup.OpenPopup();
    }

    /// <summary>
    /// Открывает всплывающее окно облигации
    /// </summary>
    /// <param name="bonds">Облигация</param>
    public void OpenBondsPopup(Bond bonds)
    {
        bondPopup.Bonds = bonds;
        bondPopup.OpenPopup();
    }

    /// <summary>
    /// Открывает всплывающее окно компании
    /// </summary>
    /// <param name="corp">Корпорация</param>
    public void OpenCompanyPopup(Corporation corp)
    {
        companyPopup.UpdateData(corp);
        companyPopup.OpenPopup();
    }

    /// <summary>
    /// Открывает всплывающее окна новости
    /// </summary>
    public void OpenNewsPopup(News news)
    {
        newsPopup.UpdateData(news);
        newsPopup.OpenPopup();
    }

    /// <summary>
    /// Закрывает все всплывающие окна
    /// </summary>
    public void ClosePopups()
    {
        sharePopup.ClosePopup();
        bondPopup.ClosePopup();
        offerPopup.ClosePopup();
        newsPopup.ClosePopup();
        investmentPopup.ClosePopup();
        currencyExchangePopup.ClosePopup();
    }


    /// <summary>
    /// Открывает диалог предложения
    /// </summary>
    public void OpenOfferDialog(Offer offer)
    {
        var dialog = offerDialog.GetComponent<UI_Dialog_Offer>();
        dialog.UpdateData(offer);
        dialog.OpenPopup();
    }

    /// <summary>
    /// Открывает диалог подтверждения
    /// </summary>
    public void OpenConfirmDialog(Action callback, GameObject content)
    {
        var dialog = confirmDialog.GetComponent<UI_Dialog_Comfirm>();
        dialog.UpdateData(callback, content);
        dialog.OpenPopup();
    }


    public void OpenProfilePage()
    {
        // TODO: реализвоать контроль над страницами
    }


    /// <summary>
    /// Показывает уведомление о недостаточном кол-ве средств на счете игрока
    /// </summary>
    public void ShowLowMoneyNotice()
    {
        lowMoneyPopup.SetActive(true);
    }
}
