using System.Collections.Generic;
using UnityEngine;

public class UI_Page_GameEvents : MonoBehaviour
{
    [SerializeField] private GameObject eventList;


    private void Awake()
    {
        Messenger.AddListener(OfferControllerNotice.OFFER_LIST_UPDATED, UpdateEventList);
    }

    private void OnEnable()
    {
        try
        {
            this.UpdateEventList();
        }
        catch
        {

        }

        Debug.Log(string.Format("Open event page\nname object: {0}", this.gameObject.name));
    }

    private void OnDisable()
    {
        Debug.Log(string.Format("Close event page\nname object: {0}", this.gameObject.name));
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(OfferControllerNotice.OFFER_LIST_UPDATED, UpdateEventList);
    }


    public void UpdateEventList()
    {
        UnityHelper.DestroyAllChildren(eventList);

        List<Investment> investments = Managers.Player.Investments;
        List<Offer> offers = Controllers.Offers.ActuallyOffers;
        offers.Reverse();

        CardBuilder.CreateHeader(eventList, "Вклады", "Вклады\nВы можете просматривать здесь информацию о своих вкладах в различные компании");
        if (investments.Count > 0)
        {
            foreach (Investment investment in investments)
                CardBuilder.CreateInvestmentCard(eventList, investment);
        }
        else
        {
            CardBuilder.CreatePlugCard(eventList, "Вы пока не вложились никуда");
        }

        CardBuilder.CreateHeader(eventList, "Предложения", "Предложения\nВы можете просматривать здесь приходящие предложения по вкладу средств");
        if (offers.Count > 0)
        {
            foreach (Offer offer in offers)
                CardBuilder.CreateOfferCard(eventList, offer);
        }
        else
        {
            CardBuilder.CreatePlugCard(eventList, "Пусто");
        }

    }
}