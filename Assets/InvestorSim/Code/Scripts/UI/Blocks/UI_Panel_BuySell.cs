using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Panel_BuySell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buyCountView; // ���-�� ��������� ��� �������
    [SerializeField] private TextMeshProUGUI sellCountView; // ���-�� ��������� ��� �������

    [SerializeField] private GameObject sellPanel;

    [SerializeField] private Slider buySlider;
    [SerializeField] private Slider sellSlider;


    /// <summary>
    /// ���-�� ��������� ������� �� �������
    /// </summary>
    public int BuyCount
    {
        get => (int)Mathf.Round(buySlider.value);
        set => buySlider.value = value;
    }
    /// <summary>
    /// ���-�� ��������� ������� �� �������
    /// </summary>
    public int SellCount
    {
        get => (int)Mathf.Round(sellSlider.value);
        set => sellSlider.value = value;
    }


    public void UpdateDate(Stock stock, AssetPurchase asset)
    {
        if (asset is not null)
        {
            sellSlider.maxValue = asset.Amount;
            sellSlider.value = 0;
            OnSellCountChanged();

            sellPanel.SetActive(true);
        }
        else
        {
            sellPanel.SetActive(false);
        }

        buySlider.maxValue = (float)Math.Floor(Managers.Player.Wallet[stock.Currency] / (stock.Price * (1 + StockExchangeController.comissionRate)));
        buySlider.value = 0;
        OnBuyCountChanged();
    }


    /// <summary>
    /// ��������� ���������� � ��������� ���-�� ����� �� �������
    /// </summary>
    public void OnSellCountChanged()
    {
        sellCountView.text = string.Format("{0}", SharpHelper.AddNumSpaces((int)Mathf.Round(sellSlider.value)));
    }

    /// <summary>
    /// ��������� ���������� � ��������� ���-�� ����� �� �������
    /// </summary>
    public void OnBuyCountChanged()
    {
        buyCountView.text = string.Format("{0}", SharpHelper.AddNumSpaces((int)Mathf.Round(buySlider.value)));
    }


    /// <summary>
    /// ����������� ���-�� ��������� ��������� �� ������� �� 1 (+1)
    /// </summary>
    public void BuyCountIncrement()
    {
        buySlider.value += 1;
    }
    /// <summary>
    /// ��������� ���-�� ��������� ��������� �� ������� �� 1 (+1)
    /// </summary>
    public void BuyCountDecrement()
    {
        buySlider.value -= 1;
    }
    /// <summary>
    /// ����������� ���-�� ��������� ��������� �� ������� �� 1 (-1)
    /// </summary>
    public void SellCountIncrement()
    {
        sellSlider.value += 1;
    }
    /// <summary>
    /// ��������� ���-�� ��������� ��������� �� ������� �� 1 (-1)
    /// </summary>
    public void SellCountDecrement()
    {
        sellSlider.value -= 1;
    }
}
