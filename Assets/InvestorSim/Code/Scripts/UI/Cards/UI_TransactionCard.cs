using UnityEngine;
using TMPro;

public class UI_TransactionCard : UI_Card
{
    [SerializeField] private TMP_Text textCaption;
    [SerializeField] private TMP_Text textSum;

    private Transaction _transaction;

    public void UpdateData(Transaction transaction)
    {
        // TODO: упростить код

        textCaption.text = transaction.Type switch
        {
            TransactionType.Buy => "Покупка",
            TransactionType.Sell => "Продажа",
            TransactionType.Swap => "Обмен",
            _ => null
        };

        IAsset asset = Controllers.Assets.FindAsset(transaction.ItemId);
        if (asset is not null)
        {

            textCaption.text += " " + asset switch
            {
                Shares => string.Format("{0} акций {1}", transaction.Quantity.ShortenNumberString(), (asset as Shares).Ticket),
                Bond => string.Format("{0} облигаций {1}", transaction.Quantity.ShortenNumberString(), (asset as Bond).Ticket),
                _ => null
            };
        }
        else
        {
            textCaption.text += " " + transaction.ItemId switch
            {
                string => string.Format(" валюты {0} > {1}", transaction.Currency, transaction.ItemId),
                _ => null
            };
        }

        textSum.text = transaction.Type switch
        {
            TransactionType.Buy => "-",
            TransactionType.Sell => "+",
            TransactionType.Swap => "",
            _ => null
        };

        if (transaction.Type != TransactionType.Swap)
            textSum.text += string.Format("{0} {1}", (transaction.Comission + transaction.Price * transaction.Quantity).AddNumSpaces(), transaction.Currency);
        else
            textSum.text += string.Format("{0} {1}", (transaction.Comission + transaction.Quantity).AddNumSpaces(), transaction.ItemId);

        if (transaction.Type == TransactionType.Sell)
            textSum.color = Color.green;


        _transaction = transaction;
    }
}