using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_AssetCard : UI_Card
{
    [SerializeField] private Image imageLogo;
    [SerializeField] private TMP_Text textAssetName;

    private IAsset _asset;

    public void UpdateData(IAsset asset)
    {
        _asset = asset;

        textAssetName.text = asset switch
        {
            Shares => string.Format("Акции {0}", (asset as Shares).CorporationName), // TODO: дополнить и другими видами активов
            null => "Null Error",
            _ => asset.Ticket
        };
    }

    public override void OnClick()
    {
        base.OnClick();

        switch (_asset)
        {
            case Shares:
                Managers.UI.OpenSharePopup(_asset as Shares);
                break;
            case Bond:
                Managers.UI.OpenBondsPopup(_asset as Bond);
                break;
        }
    }
}
