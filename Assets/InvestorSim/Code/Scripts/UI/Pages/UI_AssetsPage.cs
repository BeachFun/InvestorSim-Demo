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
    /// Обновление раздела при входе
    /// </summary>
    private void OnEnable()
    {
        try
        {
            UpdateAssetList();
        }
        catch
        {
            // TODO: try-catch временное решение. Переделать
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
    /// Открывает раздел активов
    /// </summary>
    public void Open()
    {
        Debug.Log(string.Format("Open asset section\nname object: {0}", this.gameObject.name));
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Закрывает раздел активов
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

        CardBuilder.CreateHeader(assetList, "Акции", "Акции\nЦенные бумаги, которые позволяют получить часть прибыли компании или долю при разделе ее имущества.");
        if (shareAssets.Count > 0)
        {
            foreach (AssetPurchase asset in shareAssets)
                CardBuilder.CreateAssetCard(assetList, asset);
        }
        else
        {
            CardBuilder.CreatePlugCard(assetList, "Нет купленных акций");
        }

        CardBuilder.CreateHeader(assetList, "Облигации", "Облигации\nДолговые ценные бумаги, которые выпускаются государством или компаниями, чтобы получить деньги на развитие бизнеса.");
        if (bondsAssets.Count > 0)
        {
            foreach (AssetPurchase asset in bondsAssets)
                CardBuilder.CreateAssetCard(assetList, asset);
        }
        else
        {
            CardBuilder.CreatePlugCard(assetList, "Нет купленных облигаций");
        }
    }
}
