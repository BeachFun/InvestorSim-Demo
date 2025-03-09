using UnityEngine;
using TMPro;

public class UI_BondInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI profitToMaturity;
    [SerializeField] private TextMeshProUGUI maturityDate;
    [SerializeField] private TextMeshProUGUI nominal;
    [SerializeField] private TextMeshProUGUI depreciation;

    private Bond _bond;

    public void UpdateData(Bond bond)
    {
        if (profitToMaturity is not null)
            profitToMaturity.text = string.Format("{0:p2}", bond.CalcProfitPrecentToMaturity());

        if (maturityDate is not null)
            maturityDate.text = string.Format("{0}.{1}.{2}", bond.MaturityDate.Day, bond.MaturityDate.Month, bond.MaturityDate.Year);

        if (nominal is not null)
            nominal.text = string.Format("{0}", SharpHelper.AddNumSpaces(bond.NominalValue));

        if (depreciation is not null)
            depreciation.text = string.Format("{0}", bond.IsDepreciation ? "есть" : "нет");

        this._bond = bond;
    }
}
