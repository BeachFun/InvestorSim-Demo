using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Popup_Share : UI_Popup
{
    [Header("Верхняя часть")] // с информацией главной информацией акции
    [SerializeField] private TextMeshProUGUI caption; // Company Name
    [SerializeField] private TextMeshProUGUI subcaption; // Share Type
    [Header("Панель Price Info")]
    [SerializeField] private TextMeshProUGUI currentPrice;
    [SerializeField] private TextMeshProUGUI dailyChangePriceInfo;
    [SerializeField] private TextMeshProUGUI ticket;
    [SerializeField] private UI_PriceGraph graph;
    [Header("Панель About Company")] // Поля, MaxFontSize которых нужно менять
    [SerializeField] private TextMeshProUGUI description; // Company Description | Company Info
    [SerializeField] private TextMeshProUGUI issue; // эмиссия акции
    [SerializeField] private TextMeshProUGUI count; // доступное кол-во акции
    [SerializeField] private TextMeshProUGUI issuer; // эмиссия акции
    [SerializeField] private TextMeshProUGUI country;
    [SerializeField] private TextMeshProUGUI stockExchangeInfo;
    [Header("Панель Dividends")]
    [SerializeField] private GameObject divList;
    [SerializeField] private Color lastDividends;
    [SerializeField] private Color nextDividends;
    [Header("Панель Other Info")]
    [Header("Панель Asset Info")]
    [SerializeField] private GameObject assetInfoPanel;
    [SerializeField] private TextMeshProUGUI assetCount;
    [SerializeField] private TextMeshProUGUI priceChange;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private TextMeshProUGUI changeInfo;
    [Header("Самостоятельные компоненты")]
    [SerializeField] private UI_Panel_BuySell panelBuySell;


    /// <summary>
    /// Устанавливает акцию, которую представляет это окно и обновляет инфу в окне. Ссылка на акцию, которая представляет окно
    /// </summary>
    public Shares Shares
    {
        get => _shares;
        set
        {
            _shares = value;

            // Получение информации об приобретении этой акции игроком | null - такой акции у игрока нет
            _asset = Managers.Player.FindAsset(AssetType.Shares, value);

            UpdateData();
        }
    }


    private Shares _shares; // ссылка на акцию
    private AssetPurchase _asset; // информация об акции как активе игрока

    private void Awake()
    {
        // Изменение размеров текстовых блоков в разделе дополнительной информации (Other Info) для адаптивного интерфейса
        float fontSize = subcaption.fontSize;
        description.fontSizeMax = fontSize;
        stockExchangeInfo.fontSizeMax = fontSize;
    }

    private void OnEnable()
    {
        Messenger.AddListener(StockExchangeNotice.STOCK_BUY_ACCESS, OnBuySellAccess);
        Messenger.AddListener(StockExchangeNotice.STOCK_SELL_ACCESS, OnBuySellAccess);

        UpdateData();
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(StockExchangeNotice.STOCK_BUY_ACCESS, OnBuySellAccess);
        Messenger.RemoveListener(StockExchangeNotice.STOCK_SELL_ACCESS, OnBuySellAccess);

        _asset = null;
        _shares = null;
    }


    public override void OpenPopup()
    {
        base.OpenPopup();
        Messenger<UI_Popup_Share>.Broadcast(UINotice.SHARE_POPUP_OPENED, this);
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
        Messenger<UI_Popup_Share>.Broadcast(UINotice.SHARE_POPUP_CLOSED, this);
    }


    /// <summary>
    /// Вызывается при купле/продаже. Обновляет окно и сохраняет старые выбранные данные
    /// </summary>
    private void OnBuySellAccess()
    {
        int buyCount = panelBuySell.BuyCount;
        int sellCount = panelBuySell.SellCount;

        Shares = _shares;

        panelBuySell.BuyCount = buyCount;
        panelBuySell.SellCount = sellCount;
    }


    /// <summary>
    /// Обновляет информацию во всплывающем окне | Обновляется при изменении свойства Shares
    /// </summary>
    private void UpdateData()
    {
        if (this.Shares == null) throw new Exception("Всплывающее окно не ссылается на акцию");

        panelBuySell.UpdateDate(_shares, _asset);

        UpdateMainInfo();
        UpdatePriceInfo();
        UpdateAboutCompany();
        UpdateDividendsInfo();
        UpdateAssetBox();
    }


    /// <summary>
    /// Обновляет Header окна: наименование компании и тип акции
    /// </summary>
    private void UpdateMainInfo()
    {
        caption.text = _shares.CorporationName;
        switch (_shares.Type)
        {
            case ShareType.Ordinary:
                subcaption.text = "обыкновенные";
                break;
            case ShareType.Privileged:
                subcaption.text = "привилегированные";
                break;
            default:
                subcaption.text = "";
                break;
        }
    }

    /// <summary>
    /// Заполнение информацией о ценах | Price Info Panel
    /// </summary>
    private void UpdatePriceInfo()
    {
        string ticket = _shares.Ticket;
        decimal day1Delta = Controllers.Assets.GetDay1Delta(_shares);
        decimal day1DeltaPrecent = Controllers.Assets.GetDay1DeltaPrecent(_shares);

        this.ticket.text = ticket;
        this.currentPrice.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(_shares.Price), _shares.Currency);

        if (day1Delta > 0)
        {
            dailyChangePriceInfo.text = string.Format("+{0} {1} | +{2:p2}", SharpHelper.AddNumSpaces(day1Delta), _shares.Currency, day1DeltaPrecent);
            dailyChangePriceInfo.color = Color.green;
        }
        else if (day1Delta == 0)
        {
            dailyChangePriceInfo.text = string.Format("+{0} {1} | +{2:p2}", SharpHelper.AddNumSpaces(day1Delta), _shares.Currency, day1DeltaPrecent);
            dailyChangePriceInfo.color = Color.gray;
        }
        else
        {
            dailyChangePriceInfo.text = string.Format("{0} {1} | {2:p2}", SharpHelper.AddNumSpaces(day1Delta), _shares.Currency, day1DeltaPrecent);
            dailyChangePriceInfo.color = Color.red;
        }

        // TODO: Упростить условную конструкцию

        graph.Data = Controllers.Assets.GetPriceStory(_shares);
        graph.DrowGraph();
    }

    /// <summary>
    /// Заполнение дополнительной информацией | Other Info Panel
    /// </summary>
    private void UpdateAboutCompany()
    {
        Corporation corp = Controllers.Corporations.FindCorporation(_shares.CorporationName);

        if (corp is null) return; // TODO: сделать не отображение данных при отсутствии сведений о компании

        description.text = corp.Description;
        issue.text = string.Format("{0} шт", SharpHelper.AddNumSpaces(_shares.Issue));
        count.text = string.Format("{0} шт", SharpHelper.AddNumSpaces(_shares.Amount));
        issuer.text = corp.Name;
        stockExchangeInfo.text = string.Format("{0}", _shares.StockExchange[0]);

        country.text = string.Format("{0}", corp.Country switch
        {
            Country.Russia => "Россия",
            Country.Norway => "Норвегия",
            Country.Italy => "Италия",
            Country.USA => "США",
            _ => ""
        });
    }

    /// <summary>
    /// Заполняет блок с данными о дивидендах
    /// </summary>
    private void UpdateDividendsInfo()
    {
        UnityHelper.DestroyAllChildren(divList);

        if (_shares.Dividends.Count > 0)
        {
            foreach (Dividend div in _shares.Dividends)
            {
                GameObject divPanel = Instantiate(Resources.Load("UI/Panel_Dividend"), divList.transform) as GameObject;
                var textPanel = divPanel.GetComponent<UI_TextPanel>();

                textPanel.Texts[0].text = div.CutOffDate.ToMyFormat();
                textPanel.Texts[1].text = string.Format("{0:f2} {1}", div.DividendValue, _shares.Currency);

                if (div.CutOffDate > Managers.Game.Date)
                {
                    textPanel.Texts[0].color = nextDividends;
                    textPanel.Texts[1].color = nextDividends;
                }
                else
                {
                    textPanel.Texts[0].color = lastDividends;
                    textPanel.Texts[1].color = lastDividends;
                }
            }
        }
        else
        {
            var plugInfo = CardBuilder.CreatePlugCard(divList, "Выплат еще не было").GetComponent<UI_PlugCard>();
            plugInfo.Caption.color = Color.gray;
            plugInfo.Caption.fontSize = plugInfo.Caption.fontSize * 0.7f;
        }
    }

    /// <summary>
    /// Заполнение информации об активах игрока, если есть | Asset Info Panel
    /// </summary>
    private void UpdateAssetBox()
    {
        if (_asset is not null)
        {
            assetCount.text = string.Format("{0} шт", _asset.Amount.AddNumSpaces());

            priceChange.text = string.Format("{0} {2} -> {1} {2}", _asset.CalcAvgPrice().AddNumSpaces(), _shares.Price.AddNumSpaces(), _shares.Currency);

            cost.text = string.Format("{0} {1}", (_asset.Amount * _shares.Price).AddNumSpaces(), _shares.Currency);

            // Изменение информации о доходности
            decimal profit = _asset.CalcProfitPrice();
            decimal profitRate = _asset.CalcProfitPricePrecent();
            if (profit > 0)
            {
                changeInfo.text = string.Format("+{0} {1} | +{2:p2}", SharpHelper.AddNumSpaces(profit), _shares.Currency, profitRate);
                changeInfo.color = Color.green;
            }
            else if (profit == 0)
            {
                changeInfo.text = string.Format("+{0} {1} | +{2:p2}", SharpHelper.AddNumSpaces(profit), _shares.Currency, profitRate);
                changeInfo.color = Color.gray;
            }
            else
            {
                changeInfo.text = string.Format("{0} {1} | {2:p2}", SharpHelper.AddNumSpaces(profit), _shares.Currency, profitRate);
                changeInfo.color = Color.red;
            }

            assetInfoPanel.SetActive(true);
        }
        else
        {
            assetInfoPanel.SetActive(false);
        }
    }



    /// <summary>
    /// Открытие окна компании
    /// </summary>
    public void OnIssuerClick()
    {
        Corporation corp = Controllers.Corporations.FindCorporation(_shares.CorporationName);
        if (corp is null) return;
        Managers.UI.OpenCompanyPopup(corp);
    }

    public void Buy()
    {
        int count = panelBuySell.BuyCount;

        if (count <= 0) return;

        decimal sum = Controllers.StockExchange.CalcSum(_shares, count);
        decimal comission = Controllers.StockExchange.CalcComission(_shares, count);


        GameObject prefub = Instantiate(Resources.Load("UI/Block_Stock_Transaction_Info")) as GameObject;
        prefub.GetComponent<UI_TransactionInfo>().UpdateData("Покупка акций", count, sum, comission, 0, sum + comission, _shares.Currency);

        Action callback = new Action(() =>
        {
            Controllers.StockExchange.BuyStock(this._shares, count);
        });


        Managers.UI.OpenConfirmDialog(callback, prefub);
    }

    public void Sell()
    {
        int count = panelBuySell.SellCount;

        if (count <= 0) return;

        decimal sum = Controllers.StockExchange.CalcSum(_shares, count);
        decimal comission = Controllers.StockExchange.CalcComission(_shares, count);
        decimal tax = Controllers.StockExchange.CalcTax(_shares, count);


        GameObject prefub = Instantiate(Resources.Load("UI/Block_Stock_Transaction_Info")) as GameObject;
        prefub.GetComponent<UI_TransactionInfo>().UpdateData("Продажа акций", count, sum, comission, tax, sum + comission, _shares.Currency);

        Action callback = new Action(() =>
        {
            Controllers.StockExchange.SellStock(this._shares, count);
        });


        Managers.UI.OpenConfirmDialog(callback, prefub);
    }
}
