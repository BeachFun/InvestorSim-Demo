using UnityEngine;
using TMPro;

public class UI_OfferCard : UI_Card
{
    // TODO: Добавить проверку, на то что к полям присоеденены компоненты и информирование системы об отсутствии

    [SerializeField] private TextMeshProUGUI offerType;
    [SerializeField] private TextMeshProUGUI offerDesc;

    public Offer Offer { get => _offer; }

    private Offer _offer;

    private void Awake()
    {
        offerDesc.fontSizeMax = offerType.fontSize;
        offerDesc.fontSizeMin = offerType.fontSize;
    }

    public void UpdateData(Offer offer)
    {
        offerType.text = offer.OfferType;
        offerDesc.text = offer.Description;

        _offer = offer;
    }

    public override void OnClick()
    {
        base.OnClick();
        Managers.UI.OpenPopup(this);
    }
}