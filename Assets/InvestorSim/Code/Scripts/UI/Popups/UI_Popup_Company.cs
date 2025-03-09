using UnityEngine;
using TMPro;

public class UI_Popup_Company : UI_Popup
{
    [SerializeField] private TMP_Text textTitle;
    [SerializeField] private TMP_Text textName;
    [SerializeField] private TMP_Text textDescription;
    [SerializeField] private TMP_Text textActivityArea;
    [SerializeField] private TMP_Text textCountry;
    [SerializeField] private TMP_Text textFoundationDate;
    [Header("Списки")]
    [SerializeField] private GameObject sharesList;
    [SerializeField] private GameObject bondsList;


    public override void OpenPopup()
    {
        base.OpenPopup();
        Messenger<UI_Popup_Company>.Broadcast(UINotice.COMPANY_POPUP_OPENED, this);
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
        Messenger<UI_Popup_Company>.Broadcast(UINotice.COMPANY_POPUP_CLOSED, this);
    }


    public void UpdateData(Corporation corp)
    {
        textTitle.text = corp.Name;
        textName.text = corp.Name;
        textDescription.text = corp.Description;
        textActivityArea.text = corp.ActivityArea;
        textCountry.text = corp.Country.ToString();
        textFoundationDate.text = corp.FoundationDate.ToString("dd.MM.yyyy");

        UnityHelper.DestroyAllChildren(sharesList);
        if (corp.SharesOrdTicket != string.Empty)
            CardBuilder.CreateAssetCard(sharesList, Controllers.Assets.FindAsset(corp.SharesOrdTicket));
        if (corp.SharesPrivTicket != string.Empty)
            CardBuilder.CreateAssetCard(sharesList, Controllers.Assets.FindAsset(corp.SharesPrivTicket));

        UnityHelper.DestroyAllChildren(bondsList);
        if (corp.Bonds.Count > 0)
            foreach (string bondTicket in corp.Bonds)
            {
                IAsset bond = Controllers.Assets.FindAsset(bondTicket);
                CardBuilder.CreateAssetCard(bondsList, bond);
            }
        else
            CardBuilder.CreatePlugCard(bondsList, "Нет информации");
    }
}