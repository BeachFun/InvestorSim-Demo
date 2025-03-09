using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Popup_Share : UI_Popup
{
    [Header("������� �����")] // � ����������� ������� ����������� �����
    [SerializeField] private TextMeshProUGUI caption; // Company Name
    [SerializeField] private TextMeshProUGUI subcaption; // Share Type
    [Header("������ Price Info")]
    [SerializeField] private TextMeshProUGUI currentPrice;
    [SerializeField] private TextMeshProUGUI dailyChangePriceInfo;
    [SerializeField] private TextMeshProUGUI ticket;
    [SerializeField] private UI_PriceGraph graph;
    [Header("������ About Company")] // ����, MaxFontSize ������� ����� ������
    [SerializeField] private TextMeshProUGUI description; // Company Description | Company Info
    [SerializeField] private TextMeshProUGUI issue; // ������� �����
    [SerializeField] private TextMeshProUGUI count; // ��������� ���-�� �����
    [SerializeField] private TextMeshProUGUI issuer; // ������� �����
    [SerializeField] private TextMeshProUGUI country;
    [SerializeField] private TextMeshProUGUI stockExchangeInfo;
    [Header("������ Dividends")]
    [SerializeField] private GameObject divList;
    [SerializeField] private Color lastDividends;
    [SerializeField] private Color nextDividends;
    [Header("������ Other Info")]
    [Header("������ Asset Info")]
    [SerializeField] private GameObject assetInfoPanel;
    [SerializeField] private TextMeshProUGUI assetCount;
    [SerializeField] private TextMeshProUGUI priceChange;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private TextMeshProUGUI changeInfo;
    [Header("��������������� ����������")]
    [SerializeField] private UI_Panel_BuySell panelBuySell;


    /// <summary>
    /// ������������� �����, ������� ������������ ��� ���� � ��������� ���� � ����. ������ �� �����, ������� ������������ ����
    /// </summary>
    public Shares Shares
    {
        get => _shares;
        set
        {
            _shares = value;

            // ��������� ���������� �� ������������ ���� ����� ������� | null - ����� ����� � ������ ���
            _asset = Managers.Player.FindAsset(AssetType.Shares, value);

            UpdateData();
        }
    }


    private Shares _shares; // ������ �� �����
    private AssetPurchase _asset; // ���������� �� ����� ��� ������ ������

    private void Awake()
    {
        // ��������� �������� ��������� ������ � ������� �������������� ���������� (Other Info) ��� ����������� ����������
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
    /// ���������� ��� �����/�������. ��������� ���� � ��������� ������ ��������� ������
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
    /// ��������� ���������� �� ����������� ���� | ����������� ��� ��������� �������� Shares
    /// </summary>
    private void UpdateData()
    {
        if (this.Shares == null) throw new Exception("����������� ���� �� ��������� �� �����");

        panelBuySell.UpdateDate(_shares, _asset);

        UpdateMainInfo();
        UpdatePriceInfo();
        UpdateAboutCompany();
        UpdateDividendsInfo();
        UpdateAssetBox();
    }


    /// <summary>
    /// ��������� Header ����: ������������ �������� � ��� �����
    /// </summary>
    private void UpdateMainInfo()
    {
        caption.text = _shares.CorporationName;
        switch (_shares.Type)
        {
            case ShareType.Ordinary:
                subcaption.text = "������������";
                break;
            case ShareType.Privileged:
                subcaption.text = "�����������������";
                break;
            default:
                subcaption.text = "";
                break;
        }
    }

    /// <summary>
    /// ���������� ����������� � ����� | Price Info Panel
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

        // TODO: ��������� �������� �����������

        graph.Data = Controllers.Assets.GetPriceStory(_shares);
        graph.DrowGraph();
    }

    /// <summary>
    /// ���������� �������������� ����������� | Other Info Panel
    /// </summary>
    private void UpdateAboutCompany()
    {
        Corporation corp = Controllers.Corporations.FindCorporation(_shares.CorporationName);

        if (corp is null) return; // TODO: ������� �� ����������� ������ ��� ���������� �������� � ��������

        description.text = corp.Description;
        issue.text = string.Format("{0} ��", SharpHelper.AddNumSpaces(_shares.Issue));
        count.text = string.Format("{0} ��", SharpHelper.AddNumSpaces(_shares.Amount));
        issuer.text = corp.Name;
        stockExchangeInfo.text = string.Format("{0}", _shares.StockExchange[0]);

        country.text = string.Format("{0}", corp.Country switch
        {
            Country.Russia => "������",
            Country.Norway => "��������",
            Country.Italy => "������",
            Country.USA => "���",
            _ => ""
        });
    }

    /// <summary>
    /// ��������� ���� � ������� � ����������
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
            var plugInfo = CardBuilder.CreatePlugCard(divList, "������ ��� �� ����").GetComponent<UI_PlugCard>();
            plugInfo.Caption.color = Color.gray;
            plugInfo.Caption.fontSize = plugInfo.Caption.fontSize * 0.7f;
        }
    }

    /// <summary>
    /// ���������� ���������� �� ������� ������, ���� ���� | Asset Info Panel
    /// </summary>
    private void UpdateAssetBox()
    {
        if (_asset is not null)
        {
            assetCount.text = string.Format("{0} ��", _asset.Amount.AddNumSpaces());

            priceChange.text = string.Format("{0} {2} -> {1} {2}", _asset.CalcAvgPrice().AddNumSpaces(), _shares.Price.AddNumSpaces(), _shares.Currency);

            cost.text = string.Format("{0} {1}", (_asset.Amount * _shares.Price).AddNumSpaces(), _shares.Currency);

            // ��������� ���������� � ����������
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
    /// �������� ���� ��������
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
        prefub.GetComponent<UI_TransactionInfo>().UpdateData("������� �����", count, sum, comission, 0, sum + comission, _shares.Currency);

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
        prefub.GetComponent<UI_TransactionInfo>().UpdateData("������� �����", count, sum, comission, tax, sum + comission, _shares.Currency);

        Action callback = new Action(() =>
        {
            Controllers.StockExchange.SellStock(this._shares, count);
        });


        Managers.UI.OpenConfirmDialog(callback, prefub);
    }
}
