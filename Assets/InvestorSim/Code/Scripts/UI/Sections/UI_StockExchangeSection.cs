using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������������ �� ���� ������ �������� �����
/// </summary>
public class UI_StockExchangeSection : MonoBehaviour
{
    [SerializeField] private GameObject stockList;


    private void Awake()
    {
        Messenger.AddListener(GameNotice.DAY_CHANGE, UpdateSection);
        Messenger.AddListener(StockExchangeNotice.DIVIDEND_GAP, UpdateSection);
        Messenger.AddListener(StockExchangeNotice.STOCK_DELISTING, UpdateSection);
        Messenger.AddListener(StartupNotice.ALL_CONTROLLERS_STARTED, UpdateSection);
    }

    private void Start()
    {
        UpdateSection();
    }

    /// <summary>
    /// ���������� ������� ��� �����
    /// </summary>
    private void OnEnable()
    {
        Debug.Log(string.Format("Open stock exchange\nname object: {0}", this.gameObject.name));
        //UpdateSection();

    }

    private void OnDisable()
    {
        Debug.Log(string.Format("Close stock exchange\nname object: {0}", this.gameObject.name));
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameNotice.DAY_CHANGE, UpdateSection);
        Messenger.RemoveListener(StockExchangeNotice.DIVIDEND_GAP, UpdateSection);
        Messenger.RemoveListener(StockExchangeNotice.STOCK_DELISTING, UpdateSection);
        Messenger.RemoveListener(StartupNotice.ALL_CONTROLLERS_STARTED, UpdateSection);
    }


    /// <summary>
    /// ��������� ������ �������� �����. �����, � �������, ����� ��������� ������ �� �����
    /// </summary>
    public void UpdateSection()
    {
        //Debug.Log("stock list updating");

        try
        {
            UnityHelper.DestroyAllChildren(stockList);

            CardBuilder.CreateHeader(stockList, "�����", "�����\n������ ������, ������� ��������� �������� ����� ������� �������� ��� ���� ��� ������� �� ���������.");
            foreach (string sharesTicket in Controllers.StockExchange.StockList[StockType.Shares])
            {
                IAsset shares = Controllers.Assets.FindAsset(sharesTicket);
                CardBuilder.CreateStockCard(stockList, shares);
            }
            CardBuilder.CreateHeader(stockList, "���������", "���������\n�������� ������ ������, ������� ����������� ������������ ��� ����������, ����� �������� ������ �� �������� �������.");
            foreach (string bondsTicket in Controllers.StockExchange.StockList[StockType.Bonds])
            {
                IAsset bonds = Controllers.Assets.FindAsset(bondsTicket);
                CardBuilder.CreateStockCard(stockList, bonds);
            }
        }
        catch
        {
            // TODO: try-catch ��������� �������. ����������
        }

        //Debug.Log("stock list updated");
    }
}
