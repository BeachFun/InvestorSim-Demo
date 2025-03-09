using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Popup_Investment : UI_Popup
{
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private TextMeshProUGUI deposit;
    [SerializeField] private TextMeshProUGUI profitability;
    [SerializeField] private TextMeshProUGUI depositDate;
    [SerializeField] private TextMeshProUGUI period;
    [SerializeField] private TextMeshProUGUI hasOutput;
    [SerializeField] private TextMeshProUGUI profit;
    [SerializeField] private TextMeshProUGUI skippedPays;


    public override void OpenPopup()
    {
        base.OpenPopup();
        Messenger<UI_Popup_Investment>.Broadcast(UINotice.INVESTMENT_POPUP_OPENED, this);
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
        Messenger<UI_Popup_Investment>.Broadcast(UINotice.INVESTMENT_POPUP_CLOSED, this);
    }


    public void UpdateData(Investment investment)
    {
        Offer offer = Controllers.Offers.FindOffer(investment.Offer);

        type.text = offer.OfferType;

        deposit.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(investment.Deposit), investment.Currency);

        profitability.text = string.Format("{0:p2} {1}", investment.PayRate, investment.Payment.ToMyFormat());

        depositDate.text = investment.ReleaseDate.ToMyFormat();

        period.text = ((investment.ReleaseDate + investment.Period) - investment.ReleaseDate).ToReadableShortTimeSpan();

        hasOutput.text = investment._canOutput ? "Есть" : "нет";

        profit.text = string.Format("{0:f2} {1} | +{2:p2}", investment.Profit, investment.Currency, investment.Profit / investment.Deposit);

        skippedPays.text = string.Format("{0}", investment.SkippedPays);
    }
}
