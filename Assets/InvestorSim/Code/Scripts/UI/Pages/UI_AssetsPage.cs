using System.Collections.Generic;
using UnityEngine;

public class UI_AssetsPage : MonoBehaviour
{
    [SerializeField] private GameObject assetList;


    private void Awake()
    {
        Messenger.AddListener(SectionNotice.SECTION_ALL_CLOSED, Close);
        Messenger.AddListener(StockExchangeNotice.STOCK_BUY_ACCESS, UpdateAssetList);
        Messenger.AddListener(GameNotice.DAY_CHANGE, UpdateAssetList);
    }

    /// <summary>
    /// ���������� ������� ��� �����
    /// </summary>
    private void OnEnable()
    {
        try
        {
            UpdateAssetList();
        }
        catch
        {
            // TODO: try-catch ��������� �������. ����������
        }

        Debug.Log(string.Format("Open assets page\nname object: {0}", this.gameObject.name));
    }

    private void OnDisable()
    {
        Debug.Log(string.Format("Close assets page\nname object: {0}", this.gameObject.name));
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(SectionNotice.SECTION_ALL_CLOSED, Close);
        Messenger.RemoveListener(StockExchangeNotice.STOCK_BUY_ACCESS, UpdateAssetList);
        Messenger.RemoveListener(GameNotice.DAY_CHANGE, UpdateAssetList);
    }


    /// <summary>
    /// ��������� ������ �������
    /// </summary>
    public void Open()
    {
        Debug.Log(string.Format("Open asset section\nname object: {0}", this.gameObject.name));
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// ��������� ������ �������
    /// </summary>
    public void Close()
    {
        this.gameObject.SetActive(false);
        Debug.Log(string.Format("Close asset section\nname object: {0}", this.gameObject.name));
    }




    public void UpdateAssetList()
    {
        UnityHelper.DestroyAllChildren(assetList);

        Dictionary<AssetType, List<AssetPurchase>> assets = Managers.Player.Assets;
        List<AssetPurchase> shareAssets = assets[AssetType.Shares];
        List<AssetPurchase> bondsAssets = assets[AssetType.Bonds];

        CardBuilder.CreateHeader(assetList, "�����", "�����\n������ ������, ������� ��������� �������� ����� ������� �������� ��� ���� ��� ������� �� ���������.");
        if (shareAssets.Count > 0)
        {
            foreach (AssetPurchase asset in shareAssets)
                CardBuilder.CreateAssetCard(assetList, asset);
        }
        else
        {
            CardBuilder.CreatePlugCard(assetList, "��� ��������� �����");
        }

        CardBuilder.CreateHeader(assetList, "���������", "���������\n�������� ������ ������, ������� ����������� ������������ ��� ����������, ����� �������� ������ �� �������� �������.");
        if (bondsAssets.Count > 0)
        {
            foreach (AssetPurchase asset in bondsAssets)
                CardBuilder.CreateAssetCard(assetList, asset);
        }
        else
        {
            CardBuilder.CreatePlugCard(assetList, "��� ��������� ���������");
        }
    }
}
