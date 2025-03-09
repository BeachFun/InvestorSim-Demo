using UnityEngine;
using TMPro;

public class UI_InvestmentCard : UI_Card
{
    [SerializeField] private TMP_Text deposit;
    [SerializeField] private TMP_Text profit;

    public Investment Investment { get; private set; }

    public void UpdateData(Investment investment)
    {
        deposit.text = string.Format("{0:f2} {1}", investment.Deposit, investment.Currency);
        profit.text = string.Format("{0:f2} {1} | +{2:p2}", investment.Profit, investment.Currency, investment.Profit / investment.Deposit);

        Investment = investment;
    }

    public override void OnClick()
    {
        base.OnClick();
        Managers.UI.OpenPopup(this);
    }
}
