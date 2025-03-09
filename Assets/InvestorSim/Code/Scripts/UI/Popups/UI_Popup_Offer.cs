using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UI_Popup_Offer : UI_Popup
{
    [Header("Информация о предложении")]
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private TextMeshProUGUI textOfferType;
    [SerializeField] private TextMeshProUGUI textPeriod;
    [Header("Табло с информацией вклада")]
    [SerializeField] private GameObject panelInvestmentInfo;
    [SerializeField] private TextMeshProUGUI textDeposit;
    [SerializeField] private TextMeshProUGUI textProfitability;
    [Header("Табло с информацией сдачи в долг")]
    [SerializeField] private GameObject panelDeptInfo;
    [SerializeField] private TextMeshProUGUI textSum;
    [SerializeField] private TextMeshProUGUI textSolvency;


    private Offer _offer;


    public override void OpenPopup()
    {
        base.OpenPopup();
        Messenger<UI_Popup_Offer>.Broadcast(UINotice.OFFER_POPUP_OPENED, this);
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
        Messenger<UI_Popup_Offer>.Broadcast(UINotice.OFFER_POPUP_CLOSED, this);
    }


    public void UpdateData(Offer offer)
    {
        textDescription.text = offer.Description;
        textOfferType.text = offer.OfferType;

        if (offer.OfferTarget is Investment)
        {
            panelInvestmentInfo.SetActive(true);
            panelDeptInfo.SetActive(false);

            var investment = offer.OfferTarget as Investment;

            textPeriod.text = investment.Period.ToReadableShortTimeSpan();
            textProfitability.text = string.Format("{0:p2} {1}", investment.PayRate, investment.Payment.ToMyFormat());
            textDeposit.text = string.Format("от {0}", offer.CanFlexibleDeposit ? offer.Min : offer.DepositList.Min());
            if (offer.Max is not null)
                textDeposit.text += string.Format(" до {0}", offer.Max.Value);
            textDeposit.text += string.Format(" {0}", offer.Currency);
        }
        else if(offer.OfferTarget is Dept)
        {
            panelInvestmentInfo.SetActive(false);
            panelDeptInfo.SetActive(true);

            var dept = offer.OfferTarget as Dept;

            textPeriod.text = (dept.MaturityDate - Managers.Game.Date).ToReadableShortTimeSpan();
            textSum.text = string.Format("{0} {1}", dept.Deposit.ShortenNumberString(), dept.Currency);
            textSolvency.text = dept.DeptorRate switch
            {
                DeptorRateType.Low => "Низкая",
                DeptorRateType.Medium => "Средняя",
                DeptorRateType.High => "Высокая",
                _ => ""
            };
        }
        else
        {
            textPeriod.text = "";
            panelInvestmentInfo.SetActive(false);
            panelDeptInfo.SetActive(false);
        }

        _offer = offer;
    }

    public void OnDisagree()
    {
        if (Controllers.Offers.CancelOffer(_offer))
            Messenger<Offer>.Broadcast(OfferControllerNotice.OFFER_DISAGREE, _offer);
    }

    public void OnAgree()
    {
        if (_offer.OfferTarget is Investment)
        {
            Managers.UI.OpenOfferDialog(_offer);
        }
        else if (_offer.OfferTarget is Dept)
            Controllers.Offers.AcceptOffer((_offer.OfferTarget as Dept).Deposit, _offer);
    }
}