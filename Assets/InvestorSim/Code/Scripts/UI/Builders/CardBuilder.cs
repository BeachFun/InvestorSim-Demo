using UnityEngine;
using TMPro;

internal class CardBuilder : MonoBehaviour
{
    // TODO: дать сводку классу и переделать ее у методов этого класса

    /// <summary>
    /// Создает карточку актива игрока в определенном GameObject
    /// </summary>
    /// <param name="parent">Родительский объект</param>
    /// <param name="shares">Актив игрока</param>
    public static void CreateAssetCard(GameObject parent, AssetPurchase playerAsset)
    {
        Object prefubUI;
        GameObject cardObj;

        IAsset asset = Controllers.Assets.FindAsset(playerAsset.Asset);
        if (asset == null) throw new System.Exception("Неизвестный актив");

        switch (asset)
        {
            case Shares:
                prefubUI = Resources.Load("UI/Cards/Card_Asset_Shares");
                cardObj = Instantiate(prefubUI, parent.transform) as GameObject;
                cardObj.GetComponent<UI_Assets_ShareCard>().UpdateData(asset as Shares);
                break;
            case Bond:
                prefubUI = Resources.Load("UI/Cards/Card_Asset_Bonds");
                cardObj = Instantiate(prefubUI, parent.transform) as GameObject;
                cardObj.GetComponent<UI_Assets_BondCard>().UpdateData(asset as Bond);
                break;
        }
    }

    /// <summary>
    /// Создает карточку активов в указанном GameObject.
    /// </summary>
    /// <param name="parent">Родительский объект</param>
    /// <param name="shares">Актив</param>
    public static void CreateStockCard(GameObject parent, IAsset asset)
    {
        Object prefubUI;
        GameObject stockCardObj;

        switch (asset)
        {
            case Shares:
                prefubUI = Resources.Load("UI/Cards/Card_Stock_Shares");
                stockCardObj = Instantiate(prefubUI, parent.transform) as GameObject;
                stockCardObj.GetComponent<UI_ShareCard>().UpdateData(asset as Shares);
                break;
            case Bond:
                prefubUI = Resources.Load("UI/Cards/Card_Stock_Bonds");
                stockCardObj = Instantiate(prefubUI, parent.transform) as GameObject;
                stockCardObj.GetComponent<UI_BondCard>().UpdateData(asset as Bond);
                break;
        }
    }

    /// <summary>
    /// Создает заголовок в указанном GameObject.
    /// </summary>
    /// <param name="parent">Родительский объект для карточки</param>
    /// <param name="caption">Название заголовка</param>
    public static GameObject CreateHeader(GameObject parent, string caption, string tooltipTextLeft = null, string tooltipTextRight = null)
    {
        return CreateHeader(parent, caption, Color.grey, HorizontalAlignmentOptions.Center, tooltipTextLeft, tooltipTextRight);
    }
    /// <summary>
    /// Создает заголовок в определенном GameObject с указанными фоновым цветом и расположением текста отностильно горизонтали
    /// </summary>
    /// <param name="parent">Родительский объект</param>
    /// <param name="caption">Заголовок</param>
    /// <param name="backgroundColor">Цвет фона</param>
    /// <param name="horizontalAlignment">Располжение текста по горизонтали</param>
    /// <returns></returns>
    public static GameObject CreateHeader(GameObject parent, string caption, Color backgroundColor, HorizontalAlignmentOptions horizontalAlignment, string tooltipTextLeft = null, string tooltipTextRight = null)
    {
        Object prefubUI = Resources.Load("UI/Header");
        var headerObj = Instantiate(prefubUI, parent.transform) as GameObject;

        var info = headerObj.GetComponent<UI_Header>();
        info.caption.text = caption;
        info.backgroundColor = backgroundColor;
        info.horizontalAlignment = horizontalAlignment;

        if (tooltipTextLeft is not null || tooltipTextRight is not null)
        {
            info.showingTooltip = true;
        }
        info.tooltip.infoLeft = tooltipTextLeft;
        info.tooltip.infoRight = tooltipTextRight;

        return headerObj;
    }


    /// <summary>
    /// Создает карточку предложения на странице событий
    /// </summary>
    /// <param name="parent">Родительский объект</param>
    /// <param name="offer"></param>
    public static GameObject CreateOfferCard(GameObject parent, Offer offer)
    {
        Object prefubUI = Resources.Load("UI/Cards/Card_Offer");
        var cardObj = Instantiate(prefubUI, parent.transform) as GameObject;
        cardObj.GetComponent<UI_OfferCard>().UpdateData(offer);
        return cardObj;
    }

    /// <summary>
    /// Карточка вклада в пирамиду
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="investment"></param>
    public static GameObject CreateInvestmentCard(GameObject parent, Investment investment)
    {
        Object prefubUI = Resources.Load("UI/Cards/Card_Investment");
        var cardObj = Instantiate(prefubUI, parent.transform) as GameObject;
        cardObj.GetComponent<UI_InvestmentCard>().UpdateData(investment);
        return cardObj;
    }

    /// <summary>
    /// Карточка транзакции
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public static GameObject CreateTransactionCard(GameObject parent, Transaction transaction)
    {
        Object prefubUI = Resources.Load("UI/Cards/Card_Transaction");
        var cardObj = Instantiate(prefubUI, parent.transform) as GameObject;
        cardObj.GetComponent<UI_TransactionCard>().UpdateData(transaction);
        return cardObj;
    }

    /// <summary>
    /// Карточка новости
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="news"></param>
    /// <returns></returns>
    public static GameObject CreateNewsCard(GameObject parent, News news)
    {
        Object prefubUI = Resources.Load("UI/Cards/Card_News");
        var cardObj = Instantiate(prefubUI, parent.transform) as GameObject;
        cardObj.GetComponent<UI_NewsCard>().UpdateData(news);
        return cardObj;
    }

    /// <summary>
    /// Карточка актива
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="asset"></param>
    /// <returns></returns>
    public static GameObject CreateAssetCard(GameObject parent, IAsset asset)
    {
        if (asset is null) return null;

        Object prefubUI = Resources.Load("UI/Cards/Card_Asset");
        var cardObj = Instantiate(prefubUI, parent.transform) as GameObject;
        cardObj.GetComponent<UI_AssetCard>().UpdateData(asset);
        return cardObj;
    }

    /// <summary>
    /// Карточка заглушка
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="caption"></param>
    /// <returns></returns>
    public static GameObject CreatePlugCard(GameObject parent, string caption)
    {
        Object prefubUI = Resources.Load("UI/Cards/Card_Plug");
        var cardObj = Instantiate(prefubUI, parent.transform) as GameObject;
        cardObj.GetComponent<UI_PlugCard>().UpdateData(caption);
        return cardObj;
    }
}
