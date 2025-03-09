using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IGameManager
{
    [Header("��������� �� ������� ���������")]
    [SerializeField] private UI_TabGroup tabGroup;
    [Header("�������������� ��������")]
    [SerializeField] private UI_Page_Story storyPage;
    [Header("������� ������ � ������������")]
    [SerializeField] private GameObject RUB;
    [SerializeField] private GameObject USD;
    [SerializeField] private GameObject EUR;
    [SerializeField] private TextMeshProUGUI Date;
    [Header("���������")]
    [SerializeField] private TextMeshProUGUI DaysInGame;
    [Header("������������ ����")]
    [Tooltip("������ �� ������� ����� �������������� Popups")]
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
    [Header("���������� ����")]
    [SerializeField] private GameObject offerDialog;
    [SerializeField] private GameObject confirmDialog;
    [Header("������� ����������")]
    [SerializeField] private Image newNewsMark;
    [SerializeField] private Image newOfferMark;
    [Header("������ ������")]
    [Tooltip("�������� ���������� ���")]
    [SerializeField] private GameObject dayChangeScreenSaver;
    [Header("��������� ������� ����������")]
    [Tooltip("���� ������� ��� ������ �������")]
    [SerializeField] public Color lowPower = Color.green;
    [Tooltip("���� ������� ��� ������� ������� ����")]
    [SerializeField] public Color mediumPower = Color.yellow;
    [Tooltip("���� ������� ��� ������� �������")]
    [SerializeField] public Color highPower = Color.red;
    [Tooltip("���� ���� �������")]
    [SerializeField] public Color color = Color.red;


    public ManagerStatus status { get; private set; }
    public STController Tooltip
    {
        get => tooltip;
    }


    private UI_Popup _prevPopup;
    private UI_Popup _currPopup;


    #region ������ ������� � �������������

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
            // ������� �� ������ �������
            Messenger.RemoveListener(GameNotice.DAY_CHANGE, this.DayChangeTrigger);
            Messenger.RemoveListener(GameNotice.WALLET_CHANGE, this.WalletChangeTrigger);
            Messenger.RemoveListener(StartupNotice.ALL_MANAGERS_STARTED, this.GameAwakedTrigger);
            Messenger.RemoveListener(StartupNotice.ALL_CONTROLLERS_STARTED, this.AllControllersStartedTrigger);
            Messenger.RemoveListener(GameNotice.LOW_MONEY_IN_WALLET, this.ShowLowMoneyNotice);
            // ������� �� ������� ��������� �� ������������ ������
            Messenger<UI_Popup>.RemoveListener(UINotice.SHARE_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.BOND_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.OFFER_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.NEWS_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.INVESTMENT_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.CURRENCY_EXCHANGE_POPUP_OPENED, this.PopupOpenedTrigger);
            Messenger<UI_Popup>.RemoveListener(UINotice.SETTINGS_POPUP_OPENED, this.PopupOpenedTrigger);
            // ������� �� ������� �������� ����������� ����
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

        // �������� �� ������ �������
        Messenger.AddListener(GameNotice.DAY_CHANGE, this.DayChangeTrigger);
        Messenger.AddListener(GameNotice.WALLET_CHANGE, this.WalletChangeTrigger);
        Messenger.AddListener(StartupNotice.ALL_MANAGERS_STARTED, this.GameAwakedTrigger);
        Messenger.AddListener(StartupNotice.ALL_CONTROLLERS_STARTED, this.AllControllersStartedTrigger);
        Messenger.AddListener(GameNotice.LOW_MONEY_IN_WALLET, this.ShowLowMoneyNotice);
        // �������� �� ������� �������� ����������� ����
        Messenger<UI_Popup>.AddListener(UINotice.SHARE_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.BOND_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.OFFER_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.NEWS_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.INVESTMENT_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.CURRENCY_EXCHANGE_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.SETTINGS_POPUP_OPENED, this.PopupOpenedTrigger);
        Messenger<UI_Popup>.AddListener(UINotice.COMPANY_POPUP_OPENED, this.PopupOpenedTrigger);
        // �������� �� ������� �������� ����������� ����
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


    #region �������� �������

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
                //Debug.Log("�������� ���� �������"); // TODO: ����������
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
    /// ��������� ����������� ���� � ������� � ������� ��������
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
    /// ��������� ������������ ���� �����������
    /// </summary>
    /// <param name="offer">�����������</param>
    public void OpenOfferPopup(Offer offer)
    {
        //Instantiate(popupCanvas, offerPopup.transform);
        offerPopup.UpdateData(offer);
        offerPopup.OpenPopup();
    }

    /// <summary>
    /// ��������� ����������� ���� ������
    /// </summary>
    /// <param name="investment">�������� � ��������</param>
    public void OpenInvestmentPopup(Investment investment)
    {
        investmentPopup.UpdateData(investment);
        investmentPopup.OpenPopup();
    }

    /// <summary>
    /// ��������� ����������� ���� ������ ������
    /// </summary>
    public void OpenCurrencyExchPopup()
    {
        currencyExchangePopup.OpenPopup();
    }

    /// <summary>
    /// ��������� ����������� ���� �����
    /// </summary>
    /// <param name="shares">�����</param>
    public void OpenSharePopup(Shares shares)
    {
        sharePopup.Shares = shares;
        sharePopup.OpenPopup();
    }

    /// <summary>
    /// ��������� ����������� ���� ���������
    /// </summary>
    /// <param name="bonds">���������</param>
    public void OpenBondsPopup(Bond bonds)
    {
        bondPopup.Bonds = bonds;
        bondPopup.OpenPopup();
    }

    /// <summary>
    /// ��������� ����������� ���� ��������
    /// </summary>
    /// <param name="corp">����������</param>
    public void OpenCompanyPopup(Corporation corp)
    {
        companyPopup.UpdateData(corp);
        companyPopup.OpenPopup();
    }

    /// <summary>
    /// ��������� ����������� ���� �������
    /// </summary>
    public void OpenNewsPopup(News news)
    {
        newsPopup.UpdateData(news);
        newsPopup.OpenPopup();
    }

    /// <summary>
    /// ��������� ��� ����������� ����
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
    /// ��������� ������ �����������
    /// </summary>
    public void OpenOfferDialog(Offer offer)
    {
        var dialog = offerDialog.GetComponent<UI_Dialog_Offer>();
        dialog.UpdateData(offer);
        dialog.OpenPopup();
    }

    /// <summary>
    /// ��������� ������ �������������
    /// </summary>
    public void OpenConfirmDialog(Action callback, GameObject content)
    {
        var dialog = confirmDialog.GetComponent<UI_Dialog_Comfirm>();
        dialog.UpdateData(callback, content);
        dialog.OpenPopup();
    }


    public void OpenProfilePage()
    {
        // TODO: ����������� �������� ��� ����������
    }


    /// <summary>
    /// ���������� ����������� � ������������� ���-�� ������� �� ����� ������
    /// </summary>
    public void ShowLowMoneyNotice()
    {
        lowMoneyPopup.SetActive(true);
    }
}
