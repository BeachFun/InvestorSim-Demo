using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Page_Story : MonoBehaviour
{
    [Tooltip("Объект в котором будут генерироваться карточки")]
    [SerializeField] private GameObject content;

    private List<Transaction> _transactions;

    private void Awake()
    {
        Messenger.AddListener(StartupNotice.GAME_STARTED, UpdateData);
        Messenger.AddListener(TransactionNotice.TRANSACTION_LIST_UPDATED, UpdateData);
        Messenger.AddListener(GameNotice.DAY_CHANGE, UpdateData);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(StartupNotice.GAME_STARTED, UpdateData);
        Messenger.RemoveListener(TransactionNotice.TRANSACTION_LIST_UPDATED, UpdateData);
        Messenger.RemoveListener(GameNotice.DAY_CHANGE, UpdateData);
    }

    public void UpdateData()
    {
        _transactions = Controllers.Transactions.Story;

        UnityHelper.DestroyAllChildren(content);
        if (_transactions.Count > 0)
        {
            DateTime date = DateTime.MinValue;

            for (int i = _transactions.Count - 1; i >= 0 ; i--)
            {
                if (date != _transactions[i].Date || date == DateTime.MinValue)
                {
                    date = _transactions[i].Date;
                    CardBuilder.CreateHeader(content, date.ToMyFormat(), new Color(0, 0, 0, 0), TMPro.HorizontalAlignmentOptions.Right);
                }

                CardBuilder.CreateTransactionCard(content, _transactions[i]);
            }
        }
        else
        {
            CardBuilder.CreatePlugCard(content, "Вы пока еще не совершили ни одной операции");
        }
    }
}