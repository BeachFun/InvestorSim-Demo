using System;
using System.Linq;
using UnityEngine;
using TMPro;

public class UI_Popup_Bond : UI_Popup
{
    [Header("Верхняя часть")] // с информацией главной информацией облигации
    [SerializeField] private TextMeshProUGUI caption; // Bonds Name
    [SerializeField] private TextMeshProUGUI subcaption; // Bond Type
    [Header("Панель Price Info")]
    [SerializeField] private TextMeshProUGUI currentPrice;
    [SerializeField] private TextMeshProUGUI dailyChangePriceInfo;
    [SerializeField] private TextMeshProUGUI ticket;
    [SerializeField] private UI_PriceGraph graph;
    [Header("Панель Asset Info")]
    [SerializeField] private GameObject assetPanel;
    [SerializeField] private TextMeshProUGUI assetCount;
    [SerializeField] private TextMeshProUGUI priceChange;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private TextMeshProUGUI profitText;
    [Header("Панель Other Info")]
    [SerializeField] private TextMeshProUGUI issuer;
    [SerializeField] private TextMeshProUGUI issue;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI issueDate;
    [SerializeField] private TextMeshProUGUI stockExchange;
    [Header("Самостоятельные компоненты")]
    [SerializeField] private UI_BondInfo bondInfo;
    [SerializeField] private UI_CouponInfo couponInfo;
    [SerializeField] private UI_Panel_BuySell panelBuySell;


    /// <summary>
    /// Устанавливает облигацию, которую представляет это окно и обновляет инфу в окне. Ссылка на акцию, которая представляет окно
    /// </summary>
    public Bond Bonds
    {
        get => _bonds;
        set
        {
            _bonds = value;

            // Получение информации об приобретении этой облигации игроком | null - такой облигации у игрока нет
            _asset = Managers.Player.FindAsset(AssetType.Bonds, value);

            UpdateData();
        }
    }


    private Bond _bonds; // ссылка на облигацию
    private AssetPurchase _asset; // информация об облигации как активе игрока


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
        _bonds = null;
    }


    public override void OpenPopup()
    {
        base.OpenPopup();
        Messenger<UI_Popup_Bond>.Broadcast(UINotice.BOND_POPUP_OPENED, this);
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
        Messenger<UI_Popup_Bond>.Broadcast(UINotice.BOND_POPUP_CLOSED, this);
    }


    /// <summary>
    /// Вызывается при купле/продаже. Обновляет окно и сохраняет старые выбранные данные
    /// </summary>
    private void OnBuySellAccess()
    {
        int buyCount = panelBuySell.BuyCount;
        int sellCount = panelBuySell.SellCount;

        Bonds = _bonds;

        panelBuySell.BuyCount = buyCount;
        panelBuySell.SellCount = sellCount;
    }


    /// <summary>
    /// Обновляет информацию во всплывающем окне | Обновляет при изменении свойства Bonds
    /// </summary>
    private void UpdateData()
    {
        if (this.Bonds == null) throw new Exception("Всплывающее окно не ссылается на облигацию");

        UpdateMainInfo();
        UpdatePriceInfo();
        UpdateAssetInfo();
        UpdateOtherInfo();

        bondInfo.UpdateData(_bonds);
        panelBuySell.UpdateDate(_bonds, _asset);

        if (_bonds.Type == BondType.Coupon)
        {
            couponInfo.UpdateData(_bonds.Coupon, _bonds.Currency);
            couponInfo.gameObject.SetActive(true);
        }
        else
        {
            couponInfo.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Обновляет информацию в блоке с ценой облигации
    /// </summary>
    private void UpdatePriceInfo()
    {
        // TODO: сделать не отображение данных графика, только последняя цена если такая облигация не торгуется на бирже

        string ticket = _bonds.Ticket;
        decimal day1Delta = Controllers.Assets.GetDay1Delta(_bonds);
        decimal day1DeltaPrecent = Controllers.Assets.GetDay1DeltaPrecent(_bonds);

        this.ticket.text = ticket;
        this.currentPrice.text = string.Format("{0:f2} {1}", this._bonds.Price, this._bonds.Currency);

        if (day1Delta > 0)
        {
            dailyChangePriceInfo.text = string.Format("+{0} {1} | +{2:p2}", SharpHelper.AddNumSpaces(day1Delta), _bonds.Currency, day1DeltaPrecent);
            dailyChangePriceInfo.color = Color.green;
        }
        else if (day1Delta == 0)
        {
            dailyChangePriceInfo.text = string.Format("+{0} {1} | +{2:p2}", SharpHelper.AddNumSpaces(day1Delta), _bonds.Currency, day1DeltaPrecent);
            dailyChangePriceInfo.color = Color.gray;
        }
        else
        {
            dailyChangePriceInfo.text = string.Format("{0} {1} | {2:p2}", SharpHelper.AddNumSpaces(day1Delta), _bonds.Currency, day1DeltaPrecent);
            dailyChangePriceInfo.color = Color.red;
        }

        // TODO: Упростить условную конструкцию

        graph.Data = Controllers.Assets.Data[_bonds].Select(x => ((float)x.Item1, x.Item2)).ToList();
        graph.DrowGraph();
    }

    /// <summary>
    /// Обновляет Header окна: наименование облигации и тип облигации
    /// </summary>
    private void UpdateMainInfo()
    {
        caption.text = _bonds.Issuer;
        switch (_bonds.Type)
        {
            case BondType.Coupon:
                subcaption.text = "купоные";
                break;
            case BondType.Discount:
                subcaption.text = "дисконтные";
                break;
            default:
                subcaption.text = "";
                break;
        }
    }

    private void UpdateAssetInfo()
    {
        if (_asset is not null)
        {
            assetCount.text = string.Format("{0} шт", _asset.Amount.AddNumSpaces());

            priceChange.text = string.Format("{0} {2} -> {1} {2}", _asset.CalcAvgPrice().AddNumSpaces(), _bonds.Price.AddNumSpaces(), _bonds.Currency);

            cost.text = string.Format("{0} {1}", (_asset.Amount * _asset.CalcAvgPrice()).AddNumSpaces(), _bonds.Currency);

            decimal profitValue = _asset.CalcProfit();
            if (profitValue > 0)
            {
                profitText.text = string.Format("+{0} {1} | +{2:p2}", profitValue.AddNumSpaces(), _bonds.Currency, _asset.CalcProfitPrecent());
                profitText.color = Color.green;
            }
            else if (profitValue == 0)
            {
                profitText.text = string.Format("+{0} {1} | +{2:p2}", profitValue.AddNumSpaces(), _bonds.Currency, _asset.CalcProfitPrecent());
                profitText.color = Color.gray;
            }
            else
            {
                profitText.text = string.Format("{0} {1} | {2:p2}", profitValue.AddNumSpaces(), _bonds.Currency, _asset.CalcProfitPrecent());
                profitText.color = Color.red;
            }

            assetPanel.SetActive(true);
        }
        else
        {
            assetPanel.SetActive(false);
        }
    }


    private void UpdateOtherInfo()
    {
        issuer.text = _bonds.Issuer;

        issue.text = string.Format("{0}", SharpHelper.AddNumSpaces(_bonds.Issue));

        amount.text = string.Format("{0}", SharpHelper.AddNumSpaces(_bonds.Amount));

        issueDate.text = _bonds.IssueDate.ToMyFormat();

        stockExchange.text = string.Format("{0}", _bonds.StockExchange[0]);
    }


    /// <summary>
    /// Открытие окна компании
    /// </summary>
    public void OnIssuerClick()
    {
        Corporation corp = Controllers.Corporations.FindCorporation(_bonds.Issuer);
        if (corp is null) return;
        Managers.UI.OpenCompanyPopup(corp);
    }

    public void Buy()
    {
        int count = panelBuySell.BuyCount;

        if (count <= 0) return;

        decimal sum = Controllers.StockExchange.CalcSum(_bonds, count);
        decimal comission = Controllers.StockExchange.CalcComission(_bonds, count);
        decimal? NKD = Controllers.StockExchange.CalcCouponValue(_bonds, count); // Накопленный купонный доход

        GameObject prefub = Instantiate(Resources.Load("UI/Block_Stock_Transaction_Info")) as GameObject;
        if (NKD is not null)
        {
            prefub.GetComponent<UI_TransactionInfo>()
                .UpdateData("Покупка облигаций", count, sum, comission, NKD.Value, 0, sum + comission + NKD.Value, _bonds.Currency);
        }
        else
        {
            prefub.GetComponent<UI_TransactionInfo>()
                .UpdateData("Покупка облигаций", count, sum, comission, 0, sum + comission, _bonds.Currency);
        }

        Action callback = new Action(() =>
        {
            Controllers.StockExchange.BuyStock(this._bonds, panelBuySell.BuyCount);
        });

        Managers.UI.OpenConfirmDialog(callback, prefub);
    }

    public void Sell()
    {
        int count = panelBuySell.SellCount;

        if (count <= 0) return;

        decimal sum = Controllers.StockExchange.CalcSum(_bonds, count);
        decimal comission = Controllers.StockExchange.CalcComission(_bonds, count);
        decimal? NKD = Controllers.StockExchange.CalcCouponValue(_bonds); // Накопленный купонный доход
        decimal tax = Controllers.StockExchange.CalcTax(_bonds, count);


        GameObject prefub = Instantiate(Resources.Load("UI/Block_Stock_Transaction_Info")) as GameObject;
        if (NKD is not null)
        {
            prefub.GetComponent<UI_TransactionInfo>()
            .UpdateData("Продажа облигаций", count, sum, comission, NKD.Value, tax, sum + comission + NKD.Value, _bonds.Currency);
        }
        else
        {
            prefub.GetComponent<UI_TransactionInfo>()
            .UpdateData("Продажа облигаций", count, sum, comission, tax, sum + comission, _bonds.Currency);
        }

        Action callback = new Action(() =>
        {
            Controllers.StockExchange.SellStock(this._bonds, panelBuySell.SellCount);
        });


        Managers.UI.OpenConfirmDialog(callback, prefub);
    }
}