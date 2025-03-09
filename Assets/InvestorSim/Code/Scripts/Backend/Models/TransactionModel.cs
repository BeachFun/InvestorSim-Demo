using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]

/// <summary>
/// Модель данных контроллера транзакций
/// </summary>
public class TransactionModel : IGameModel
{
    public List<Transaction> Transactions
    {
        get;
        private set;
    }

    public TransactionModel()
    {
        Transactions = new List<Transaction>();
    }

    public object Clone()
    {
        return new TransactionModel()
        {
            Transactions = this.Transactions.ToList()
        };
    }
}
