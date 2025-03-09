using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер транзакций. Осуществляет переводы денег
/// </summary>
public class TransactionController : MonoBehaviour, IGameController, IDataCopable
{
    public ControllerStatus status { get; private set; }

    /// <summary>
    /// История транзакций | Список транзакций
    /// </summary>
    public List<Transaction> Story { get => _model.Transactions; }


    private TransactionModel _model;


    #region Методы запуска и Инициализации

    public IEnumerator Startup()
    {
        Debug.Log("Transaction controller starting...");
        status = ControllerStatus.Initializing;

        yield return null;

        // Если на момент запуска нет данных, то создаются данные по-умолчанию
        if (_model is null) _model = new TransactionModel();

        Messenger<Bond>.AddListener(StockExchangeNotice.BONDS_MATURITY_DATE, PayBondNominal);
        Messenger<Bond>.AddListener(StockExchangeNotice.COUPON_PAYMENT, PayBondCoupon);
        Messenger<Shares>.AddListener(StockExchangeNotice.DIVIDEND_PAYMENT, PayShareDividends);

        status = ControllerStatus.Started;
        Debug.Log("Transaction controller started...");
    }

    public void UpdateData(TransactionModel data)
    {
        _model = data;
    }

    #endregion


    /// <summary>
    /// Вносит в список транзакций новую транзакцию
    /// </summary>
    /// <param name="transaction">Данные о транзакции</param>
    public void AddTransaction(Transaction transaction)
    {
        //Debug.Log($"Transaction:\n1.Type - {transaction.Type}\n2. Name - {(transaction.Item as IAsset).Ticket}\n3. Count - {transaction.Quantity}\n" +
        //    $"4. Price - {transaction.Price}\n5. Comission - {transaction.Comission}\n6. Date - {transaction.Date}");

        _model.Transactions.Add(transaction);

        Messenger.Broadcast(TransactionNotice.TRANSACTION_LIST_UPDATED);
        Messenger<Transaction>.Broadcast(TransactionNotice.NEW_TRANSACTION, transaction);
        Messenger.Broadcast(DataNotice.TRANSACTION_MODEL_UPDATED);
    }

    /// <summary>
    /// Выплата номинала облигации при ее погашении
    /// </summary>
    private void PayBondNominal(Bond bonds)
    {
        Controllers.StockExchange.RemoveStock(bonds);

        AssetPurchase asset = Managers.Player.FindAsset(AssetType.Bonds, bonds);

        if (asset == null)
            return;

        decimal totalProfit = 0;
        foreach(PlayerAssetInfo info in asset.Buys)
        {
            totalProfit += info.Amount * bonds.NominalValue;
        }

        Managers.Player.RemoveAsset(AssetType.Bonds, bonds);
        Managers.Player.ChangeWalletMoney(bonds.Currency, totalProfit);

        Messenger.Broadcast(StockExchangeNotice.BONDS_NOMINAL_PAYOUT);
    }

    /// <summary>
    /// Выплата купона облигации. Обработчик дня выплаты купона
    /// </summary>
    private void PayBondCoupon(Bond bonds)
    {
        AssetPurchase asset = Managers.Player.FindAsset(AssetType.Bonds, bonds);

        if (asset == null)
            return;

        decimal profitStep; // от одной покупки
        decimal totalProfit = 0; // от всех покупок
        for (int i = 0; i < asset.Buys.Count; i++)
        {
            profitStep = bonds.Coupon.CalcCouponProfit(asset.Buys[i].Date) * asset.Buys[i].Amount;
            totalProfit += profitStep;
            asset.Buys[i] = new PlayerAssetInfo(asset.Buys[i].Amount, asset.Buys[i].Price, asset.Buys[i].Date, asset.Buys[i].Profit + profitStep); // TODO: делегировать изменения в сам класс
        }

        Managers.Player.ChangeWalletMoney(bonds.Currency, totalProfit); // ?
        Messenger.Broadcast(StockExchangeNotice.COUPON_RECEIPT);
    }

    /// <summary>
    /// Выплата дивидендов акции. Обработчик дня выплаты дивидендов
    /// </summary>
    private void PayShareDividends(Shares shares)
    {
        Dividend? dividend = shares.GetNextDividend();

        shares.FairPrice -= dividend.Value.DividendValue;
        shares.Price -= dividend.Value.DividendValue;
        Messenger.Broadcast(StockExchangeNotice.DIVIDEND_GAP);

        AssetPurchase asset = Managers.Player.FindAsset(AssetType.Shares, shares);

        if (asset == null && !shares.IsHolder)
            return;

        decimal profitStep; // от одной покупки
        decimal totalProfit = 0; // от всех покупок
        for (int i = 0; i < asset.Buys.Count; i++)
        {
            profitStep = dividend.Value.DividendValue * asset.Buys[i].Amount;
            totalProfit += profitStep;
            asset.Buys[i] = new PlayerAssetInfo(asset.Buys[i].Amount, asset.Buys[i].Price, asset.Buys[i].Date, profitStep); // TODO: делегировать изменения в сам класс
        }

        Managers.Player.ChangeWalletMoney(shares.Currency, totalProfit); // ?
        Messenger.Broadcast(StockExchangeNotice.DIVIDEND_RECEIPT);
    }


    public object GetDataCopy()
    {
        return _model.Clone();
    }
}